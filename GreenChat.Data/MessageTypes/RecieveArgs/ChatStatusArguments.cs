using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class ChatStatusArguments : BaseRecieveArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
        [JsonProperty("status")]
        public MessStatus Status { get; set; }
        [JsonProperty("id")]
        public int MessageId { get; set; }
    }
}