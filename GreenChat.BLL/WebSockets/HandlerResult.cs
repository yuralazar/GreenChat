using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using GreenChat.Data.Formats;

namespace GreenChat.BLL.WebSockets
{
    public struct HandlerResult
    {
        public IReadOnlyCollection<WebSocket> Sockets;
        public SendFormat Message;
    }
}