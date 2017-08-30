using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using GreenChat.DAL.Models;
using Microsoft.Extensions.Logging;

namespace GreenChat.BLL.WebSockets
{
            
    public class WebSocketConnectionManager
    {
        // Yurii Lazar - start            
        //private ConcurrentDictionary<string, WebSocket> _userSockets = new ConcurrentDictionary<string, WebSocket>();
        // changing this dictionary for private messages to send
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _userSockets 
                                = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        private readonly ILogger _logger;

        public WebSocketConnectionManager(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebSocketConnectionManager>();
        }

        private static string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public bool UserIsOnline(ApplicationUser user)
        {
            if (!_userSockets.ContainsKey(user.Id))
                return false;

            return !_userSockets[user.Id].IsEmpty;
        }

        public WebSocket GetSocketById(string id)
        {
            return (
                from pair in _userSockets
                from socketPair in pair.Value
                    where socketPair.Key == id
                select socketPair.Value
                )
                .FirstOrDefault();
        }
        
        public IReadOnlyCollection<WebSocket> GetSocketsByUser(ApplicationUser user)
        {
            return GetSocketsByUser(user.Id);
        }

        public IReadOnlyCollection<WebSocket> GetSocketsByUser(string userId)
        {
            var dict = _userSockets.ContainsKey(userId) ? _userSockets[userId] : null;
            if (dict == null) return null;
            var list = new List<WebSocket>();
            var coll = new ReadOnlyCollection<WebSocket>(list);
            list.AddRange(dict.Select(pair => pair.Value)
                              .Where(SocketIsOpened));
            RemoveNotActiveSockets(dict);
            return coll;
        }

        private static bool SocketIsOpened(WebSocket socket)
        {
            return socket.State == WebSocketState.Open || socket.State == WebSocketState.Connecting;
        }

        private void RemoveNotActiveSockets(ConcurrentDictionary<string, WebSocket> dict)
        {
            dict.Select(pair => pair.Value)
                .Where(socket => !SocketIsOpened(socket))
                .ToList()
                .ForEach(socket => RemoveSocket(socket));
        }

        public IReadOnlyCollection<WebSocket> GetSockets(List<ApplicationUser> usersTo)
        {
            var list = new List<WebSocket>();
            var coll = new ReadOnlyCollection<WebSocket>(list);
            list.AddRange(from user in usersTo
                          where _userSockets.ContainsKey(user.Id)
                          from elem in _userSockets[user.Id]
                          where SocketIsOpened(elem.Value)
                          select elem.Value);

            var notActiveSockets = from user in usersTo
                where _userSockets.ContainsKey(user.Id)
                from elem in _userSockets[user.Id]
                where !SocketIsOpened(elem.Value)
                select elem.Value;

            notActiveSockets.ToList().ForEach(socket => RemoveSocket(socket));

            return coll;
        }

        public IReadOnlyCollection<WebSocket> GetAll()
        {
            var list = new List<WebSocket>();
            var coll = new ReadOnlyCollection<WebSocket>(list);
            list.AddRange(from pair in _userSockets
                          from socketPair in pair.Value
                          select socketPair.Value);
            return coll;
        }

        public string GetId(WebSocket socket)
        {
            return (
                from pair in _userSockets
                from socketPair in pair.Value
                where socketPair.Value == socket
                select socketPair.Key
                )
                .FirstOrDefault();
        }

        public void AddSocket(ApplicationUser user, WebSocket socket)
        {
            if (_userSockets.ContainsKey(user.Id))
                _userSockets[user.Id].TryAdd(CreateConnectionId(), socket);
            else
            {
                var dict = new ConcurrentDictionary<string, WebSocket>();
                dict.TryAdd(CreateConnectionId(), socket);
                _userSockets.TryAdd(user.Id, dict);
            }
        }

        //---------------------------Remove socket methods-------------------------
        private async Task RemoveSocket(WebSocket socket)
        {
            if (socket == null || !SocketIsOpened(socket))
                return;
            try
            {

                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                                statusDescription: "Closed by the WebSocketManager",
                                                cancellationToken: CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
            }
        }

        public async Task RemoveSocket(ApplicationUser user, WebSocket socket)
        {
            _userSockets[user.Id].TryRemove(GetId(socket), out socket);
            await RemoveSocket(socket);
        }

        public async Task RemoveSocket(string id)
        {
            foreach (var pair in _userSockets)
            {
                var dict = pair.Value;
                if (dict.ContainsKey(id))
                {
                    dict.TryRemove(id, out WebSocket socket);
                    await RemoveSocket(socket);
                }
            }            
        }

        public async Task RemoveSocket(ApplicationUser user, string id)
        {
            _userSockets[user.Id].TryRemove(id, out WebSocket socket);
            await RemoveSocket(socket);
        }

        public async Task RemoveUserSockets(ApplicationUser user)
        {
            if (_userSockets.ContainsKey(user.Id))
            {
                var dict = _userSockets[user.Id];
                foreach (var pair in dict)
                {
                    dict.TryRemove(pair.Key, out WebSocket socket);
                    await RemoveSocket(socket);
                }
            }
        }

        //---------------------------End Remove socket methods-------------------------
    }

}
