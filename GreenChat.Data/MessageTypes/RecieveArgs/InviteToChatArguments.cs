using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class InviteToChatArguments : BaseRecieveArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }
        [JsonProperty("users")]
        public List<UserInfo> Users { get; set; }
    }
}