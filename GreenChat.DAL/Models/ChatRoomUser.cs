using System.Collections.Generic;

namespace GreenChat.DAL.Models
{
    public class ChatRoomUser
    {
        //Primary key
        public int ChatRoomUserID { get; set; }

        //Foreign key
        public string UserID { get; set; }
        public int ChatRoomID { get; set; }
        public bool Confirmed { get; set; }

        //Navigation properties
        public ApplicationUser User { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public virtual ICollection<ChatMessage> NavChatMessages { get; set; }
        public virtual ICollection<UnreadChatMessage> NavUnreadChatMessages1 { get; set; }
        public virtual ICollection<UnreadChatMessage> NavUnreadChatMessages2 { get; set; }
    }
}