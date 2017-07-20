using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GreenChat.WebAPI.WebSocketManagement
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public WebSocketManagerMiddleware(RequestDelegate next                         
                                          ,ILoggerFactory loggerFactory
                                          ,IServiceProvider serviceProvider)
        {
            _next = next;            
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<WebSocketManagerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogDebug("Incoming websocket connection");
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);            

            var userIdentity = context.User.Identity.Name;
            if (userIdentity == null)
            {
                _logger.LogError("Websocket connection not authorized!");
                return;
            }
            var handler = GetNewWebSocketHandler();
            await handler.OnConnected(userIdentity, socket).ConfigureAwait(false);

            await Receive(context, socket, async (result, serializedJson) =>
            {
                if (result == null) return;

                var webSocketHandler = GetNewWebSocketHandler();

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await webSocketHandler.HandleReceivingAsync(socket, result, serializedJson).ConfigureAwait(false);
                        break;
                    case WebSocketMessageType.Close:
                        await webSocketHandler.OnDisconnected(userIdentity, socket);
                        break;
                }
            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }

        private async Task Receive(HttpContext context, WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            while (!(socket.State == WebSocketState.Aborted 
                  || socket.State == WebSocketState.Closed
                  || socket.State == WebSocketState.CloseReceived
                  || socket.State == WebSocketState.CloseSent))
            {
                var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
                string serializedJson;
                WebSocketReceiveResult result = null;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        try
                        {
                            result = await socket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                            if (result != null)
                                ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        catch (WebSocketException e)
                        {
                            _logger.LogError(e.Message + " \n WebSocketErrorCode = " + e.WebSocketErrorCode);
                            var userIdentity = context.User.Identity.Name;
                            await GetNewWebSocketHandler().OnDisconnected(userIdentity, socket);
                            result = null;
                        }                       
                    }
                    while (result != null && !result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        serializedJson = await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }

                handleMessage(result, serializedJson);
            }
        }

        private WebSocketHandler GetNewWebSocketHandler()
        {
            return (WebSocketHandler)_serviceProvider.GetService(typeof(WebSocketHandler));            
        }
    }
}
