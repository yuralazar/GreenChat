using System;
using GreenChat.Data.Instances;

namespace GreenChat.DAL.Models
{
    public class PrivateMessageStatus
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PrivateMessageId { get; set; }
        public string UserId { get; set; }
        public MessStatus Status { get; set; }

        public PrivateMessage PrivateMessage { get; set; }
        public ApplicationUser User { get; set; }
    }
}