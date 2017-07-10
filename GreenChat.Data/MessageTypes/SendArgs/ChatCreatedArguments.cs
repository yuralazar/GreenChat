using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class ChatCreatedArguments : BaseSendArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
    }
}