using System;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class GetChatMessagesArguments : BaseRecieveArguments
    {
        [JsonProperty("chat")]
        public ChatInfo Chat { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

    }
}