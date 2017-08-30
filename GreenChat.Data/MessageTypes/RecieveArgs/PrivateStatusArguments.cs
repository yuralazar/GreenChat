using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class PrivateStatusArguments : BaseRecieveArguments
    {
        [JsonProperty("userTo")]
        public UserInfo UserTo { get; set; }
        [JsonProperty("status")]
        public MessStatus Status { get; set; }
        [JsonProperty("id")]
        public int MessageId { get; set; }
    }
}