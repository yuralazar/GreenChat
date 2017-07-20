using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class SendPrivateArguments : BaseSendArguments
    {
        [JsonProperty("userTo")]
        public UserInfo UserTo { get; set; }
        [JsonProperty("message")]
        public MessageInfo Message { get; set; }
        [JsonProperty("idNew")]
        public int IdNew { get; set; }
        [JsonProperty("status")]
        public MessStatus Status { get; set; }
    }
}