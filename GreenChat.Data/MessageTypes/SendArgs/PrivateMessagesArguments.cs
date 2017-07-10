using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class PrivateMessagesArguments : BaseSendArguments
    {
        [JsonProperty("messages")]
        public IEnumerable<SendPrivateArguments> Messages { get; set; }
    }
}