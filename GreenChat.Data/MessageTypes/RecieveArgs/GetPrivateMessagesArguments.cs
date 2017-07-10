using System;
using GreenChat.Data.Instances;
using Newtonsoft.Json;

namespace GreenChat.Data.MessageTypes.RecieveArgs
{
    public class GetPrivateMessagesArguments : BaseRecieveArguments
    {
        [JsonProperty("sender")]
        public UserInfo Sender { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

    }
}