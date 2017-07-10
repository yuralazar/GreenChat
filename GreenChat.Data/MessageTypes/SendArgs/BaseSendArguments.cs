using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public abstract class BaseSendArguments
    {
        // For now, there is no need to send socketId to client
        // because client needs to know just User to which he sends messages.
        //[JsonProperty("socketId")]
        //public string SocketId { get; set; }

        [JsonProperty("userFrom")]
        public UserInfo UserFrom { get; set; }

        protected BaseSendArguments()
        {            
        }

        protected BaseSendArguments(UserInfo userFrom)
        {
            UserFrom = userFrom;
        }
    }
}