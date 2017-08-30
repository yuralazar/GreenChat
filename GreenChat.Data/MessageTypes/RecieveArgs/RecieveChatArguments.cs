using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class RecieveChatArguments : BaseRecieveArguments
    {
        [JsonProperty("message")]
        public MessageInfo Message { get; set; }
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
        [JsonProperty("idNew")]
        public int IdNew { get; set; }
    }
}