using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class ChatRoomUsersArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }

        [JsonProperty("users")]
        public IEnumerable<UserInfo> Users { get; set; }

    }
}