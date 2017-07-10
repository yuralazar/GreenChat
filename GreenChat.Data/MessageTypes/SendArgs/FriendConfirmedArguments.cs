using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class FriendConfirmedArguments : BaseSendArguments
    {
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }
    }
}