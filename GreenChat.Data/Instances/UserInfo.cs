using Newtonsoft.Json;

namespace GreenChat.Data.Instances
{
    public class UserInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("online")]
        public bool Online { get; set; }    
        [JsonProperty("potential")]
        public bool Potential { get; set; }

        public UserInfo()
        {
        }

        public UserInfo(string id, string email)
        {            
            Id = id;
            Email = email;                        
        }
    }
}