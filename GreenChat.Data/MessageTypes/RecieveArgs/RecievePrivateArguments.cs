using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class RecievePrivateArguments : BaseRecieveArguments
    {
        [JsonProperty("message")]
        public MessageInfo Message { get; set; }
        [JsonProperty("userTo")]
        public UserInfo UserTo { get; set; }
    }
}