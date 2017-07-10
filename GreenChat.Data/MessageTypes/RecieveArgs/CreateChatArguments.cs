using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class CreateChatArguments : BaseRecieveArguments
    {
        [JsonProperty("chatName")]
        public string ChatName { get; set; }
    }
}