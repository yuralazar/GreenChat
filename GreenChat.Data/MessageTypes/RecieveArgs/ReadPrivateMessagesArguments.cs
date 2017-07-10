using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class ReadPrivateMessagesArguments : BaseRecieveArguments
    {
        [JsonProperty("userTo")]
        public UserInfo UserTo { get; set; }
    }
}