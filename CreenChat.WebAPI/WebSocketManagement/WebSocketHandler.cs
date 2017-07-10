using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GreenChat.Data.Formats;
using GreenChat.Data.MessageTypes;
using GreenChat.BLL.WebSockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CreenChat.WebAPI.WebSocketManagement
{
    public class WebSocketHandler
    {
        private readonly WebSocketConnectionManager _connectionManager;
        private readonly ILogger _logger;
        private readonly SendFormatFactory _sendFormatFactory;
        private readonly ChatHandler _chatHandler;

        public WebSocketHandler(WebSocketConnectionManager connectionManager
                                ,ILoggerFactory loggerFactory
                                ,SendFormatFactory sendFormatFactory
                                ,ChatHandler chatHandler)
        {
            _connectionManager = connectionManager;
            _sendFormatFactory = sendFormatFactory;
            _chatHandler = chatHandler;
            _logger = loggerFactory.CreateLogger<WebSocketHandler>();
        }
            
        //---------------------------- Connection Handlings ------------------------
        public async Task OnConnected(string userIdenity, WebSocket socket)
        {            
            try
            {
                var handlerResultList = await _chatHandler.OnConnected(userIdenity, socket);
                foreach (var res in handlerResultList)
                {
                    await SendingCycle(res.Sockets, res.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await SendMessageAsync(socket, _sendFormatFactory.ErrorMessage("OnConnected", e.Message));
            }            
        }

        public async Task OnDisconnected(string userIdenity, WebSocket socket)
        {
            try
            {
                var handlerResult = await _chatHandler.OnDisconnected(userIdenity, socket);
                await SendingCycle(handlerResult.Sockets, handlerResult.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await SendMessageAsync(socket, _sendFormatFactory.ErrorMessage("OnDisconnected", e.Message));
            }            
        }

        //---------------------------- Message sending ---------------------------------
        public async Task SendMessageAsync(WebSocket socket, SendFormat message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var serializedMessage = JsonConvert.SerializeObject(message, _sendFormatFactory.Settings);
            try
            {
                await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.UTF8.GetBytes(serializedMessage)),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }          
        }

        public async Task SendMessageAsync(string socketId, SendFormat message)
        {
            await SendMessageAsync(_connectionManager.GetSocketById(socketId), message).ConfigureAwait(false);
        }

        public async Task SendMessageToAllAsync(SendFormat message)
        {
            foreach (var pair in _connectionManager.GetAll())
            {
                if (pair.State == WebSocketState.Open)
                    await SendMessageAsync(pair, message).ConfigureAwait(false);
            }
        }

        //----------------------------Recieving------------------------------------------------
        // In this method the right handling method from ChatHandler invokes
        // If parameters from client side are wrong - sending message about this to client
        public async Task HandleReceivingAsync(WebSocket socket, WebSocketReceiveResult result, string serializedObject)
        {
            var methodName = "";
            try
            {
                var recieveFormat = JsonConvert.DeserializeObject<RecieveFormat>(serializedObject);
                var argumentsType = RecieveTypes.GetRecieveArgumentsType(recieveFormat.RecieveActionType);
                methodName = RecieveTypes.GetRecieveMethod(recieveFormat.RecieveActionType);
                var recieveArgs = JsonConvert.DeserializeObject(recieveFormat.Arguments, argumentsType);
                var method = _chatHandler.GetType().GetMethod(methodName);
            
                var handlerResult = await (Task<HandlerResult>)method.Invoke(_chatHandler, new []{ recieveArgs, socket});
                await SendingCycle(handlerResult.Sockets, handlerResult.Message);
            }
            catch (Exception e)
            {
                await SendMessageAsync(socket, _sendFormatFactory.ErrorMessage(methodName, e.Message)).ConfigureAwait(false);
                _logger.LogError(e.ToString());
            }
        }

        private async Task SendingCycle(IReadOnlyCollection<WebSocket> sockets, SendFormat message)
        {
            if (sockets == null || message == null) return;

            foreach (var socket in sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(socket, message).ConfigureAwait(false);
                }
            }
        }
    }
}