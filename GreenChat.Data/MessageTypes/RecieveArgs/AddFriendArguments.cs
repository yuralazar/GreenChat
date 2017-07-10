using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class AddFriendArguments : BaseRecieveArguments
    {
        [JsonProperty("initiator")]
        public bool Initiator { get; set; }
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }
        [JsonProperty("user")]
        public UserInfo User { get; set; }
    }
}