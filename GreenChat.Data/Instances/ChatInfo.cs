using Newtonsoft.Json;

namespace GreenChat.Data.Instances
{
    public class ChatInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public ChatInfo()
        {
        }

        public ChatInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}