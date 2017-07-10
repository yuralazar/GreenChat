using Newtonsoft.Json;

namespace GreenChat.Data.Formats
{
    public class RecieveFormat
    {
        // This number is connected with RecieveTypes
        // and indicates what method would be invoked on server
        [JsonProperty("type")]
        public int RecieveActionType { get; set; }

        // This string would be parsed into Arguments type from RecieveArguments folder
        [JsonProperty("arguments")]
        public string Arguments { get; set; }
    }

}