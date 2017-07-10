using System;

namespace GreenChat.DAL.Models
{
    public class ChatMessage
    {
        //Primary key
        public int ChatMessageID { get; set; }

        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        //Foreign key
        public int ChatRoomUserID { get; set; }
        public int ChatRoomID { get; set; }

        //Navigation properties
        public ChatRoomUser ChatRoomUser { get; set; }
        public ChatRoom ChatRoom { get; set; }

    }
}