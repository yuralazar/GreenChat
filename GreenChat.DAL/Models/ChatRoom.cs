using System.Collections.Generic;

namespace GreenChat.DAL.Models
{
    public class ChatRoom
    {
        //Primary key
        public int ChatRoomID { get; set; }

        public string Name { get; set; }

        //Foreign key
        // user that created chat
        public string UserID { get; set; }

        ////Navigation properties
        public ApplicationUser User { get; set; }

        public virtual ICollection<ChatRoomUser> NavChatRoomUsers { get; set; }
        public virtual ICollection<ChatMessage> NavChatMessages { get; set; }        
    }
}