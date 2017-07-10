using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class ChatConfirmedArguments : BaseSendArguments
    {
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
    }
}