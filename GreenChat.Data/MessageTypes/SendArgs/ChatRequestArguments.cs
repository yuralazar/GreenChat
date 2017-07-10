using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class ChatRequestArguments : BaseSendArguments
    {        

        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }

        [JsonProperty("chatUsers")]
        public IEnumerable<UserInfo> Users { get; set; }

        public ChatRequestArguments()
        {
        }

        public ChatRequestArguments(int chatId, string chatName, string userId, string userEmail) : base(new UserInfo(userId, userEmail))
        {
            Chat = new ChatInfo(chatId, chatName);
        }
    }
}