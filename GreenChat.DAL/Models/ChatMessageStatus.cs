using System;
using GreenChat.Data.Instances;

namespace GreenChat.DAL.Models
{
    public class ChatMessageStatus
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ChatMessageId { get; set; }
        public string UserId { get; set; }
        public MessStatus Status { get; set; }

        public ChatMessage ChatMessage { get; set; }
        public ApplicationUser User { get; set; }
    }
}