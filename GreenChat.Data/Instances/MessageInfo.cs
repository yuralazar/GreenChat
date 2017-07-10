using System;
using Newtonsoft.Json;

namespace GreenChat.Data.Instances
{
    public class MessageInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }        
    }    
}
