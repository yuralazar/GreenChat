using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class ConfirmChatArguments : BaseRecieveArguments
    {
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }

        [JsonProperty("invitor")]
        public UserInfo Invitor { get; set; }
    }
}