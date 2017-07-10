using System.Collections.Generic;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.SendArgs
{
    public class InitialInfo
    {
        [JsonProperty("user")]
        public UserInfo User { get; set; }

        [JsonProperty("friends")]
        public IEnumerable<UserInfo> Friends { get; set; }

        [JsonProperty("chatRooms")]
        public IEnumerable<ChatInfo> ChatRooms { get; set; }

        [JsonProperty("chatsUsers")]
        public IEnumerable<ChatRoomUsersArguments> ChatRoomsUsers { get; set; }        

        [JsonProperty("privateMessages")]
        public IEnumerable<SendPrivateArguments> PrivateMessages { get; set; }

        [JsonProperty("chatMessages")]
        public IEnumerable<SendChatArguments> ChatMessages { get; set; }

        [JsonProperty("chatRequests")]
        public IEnumerable<ChatRequestArguments> ChatRequests { get; set; }        
    }
}
