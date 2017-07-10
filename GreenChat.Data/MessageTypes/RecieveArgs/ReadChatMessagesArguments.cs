using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class ReadChatMessagesArguments : BaseRecieveArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
        [JsonProperty("userTo")]
        public UserInfo UserTo { get; set; }
    }
}