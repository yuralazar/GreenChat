using System;
using System.ComponentModel.DataAnnotations;

namespace GreenChat.DAL.Models
{
    public class UnreadPrivateMessage
    {
        //Primary key
        public int UnreadPrivateMessageID { get; set; }
        public int PrivateMessageID { get; set; }

        public string Content { get; set; }        
        public DateTimeOffset Date { get; set; }          

        //Foreign key
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }

        //Navigation properties
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}