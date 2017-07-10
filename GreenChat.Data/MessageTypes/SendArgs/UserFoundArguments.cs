using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class UserFoundArguments : BaseSendArguments
    {
        [JsonProperty("user")]
        public UserInfo User { get; set; }
    }
}