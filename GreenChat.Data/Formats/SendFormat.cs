using GreenChat.Data.MessageTypes;
using Newtonsoft.Json;

namespace GreenChat.Data.Formats
{
    public class SendFormat
    {
        [JsonProperty("type")]
        public SendActionTypes SendActionType { get; set; }
        [JsonProperty("arguments")]
        public string Arguments { get; set; }    
    }
}