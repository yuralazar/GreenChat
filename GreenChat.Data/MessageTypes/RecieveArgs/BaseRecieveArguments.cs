using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public abstract class BaseRecieveArguments
    {        
        [JsonProperty("socketId")]
        public string SocketId { get; set; }
        [JsonProperty("userFrom")]
        public UserInfo UserFrom { get; set; }        
    }
}