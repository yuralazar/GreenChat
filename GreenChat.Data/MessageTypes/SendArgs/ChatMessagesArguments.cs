using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class ChatMessagesArguments : BaseSendArguments
    {
        [JsonProperty("messages")]
        public IEnumerable<SendChatArguments> Messages { get; set; }

        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
    }
}