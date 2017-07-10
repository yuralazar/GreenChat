using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class SearchFriendArguments : BaseRecieveArguments
    {
        [JsonProperty("searchEmail")]
        public string SearchEmail { get; set; }
    }
}