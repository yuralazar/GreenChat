using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class SendChatArguments : BaseSendArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
        [JsonProperty("message")]
        public MessageInfo Message { get; set; }
        [JsonProperty("idNew")]
        public int IdNew { get; set; }
    }
}