using System;

namespace GreenChat.DAL.Models
{
    public class UnreadChatMessage
    {
        //Primary key
        public int UnreadChatMessageID { get; set; }
        public int ChatMessageID { get; set; }

        public string Content { get; set; }
        public DateTimeOffset Date { get; set; }
        
        //Foreign key
        public int ChatRoomID { get; set; }
        public int ChatRoomUserFromID { get; set; }
        public int ChatRoomUserToID { get; set; }

        //Navigation properties
        public ChatRoom ChatRoom { get; set; }
        public ChatRoomUser ChatRoomUserFrom { get; set; }
        public ChatRoomUser ChatRoomUserTo { get; set; }        

    }
}