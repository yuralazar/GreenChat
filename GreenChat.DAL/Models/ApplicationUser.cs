using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GreenChat.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        //Navigation properties
        public virtual ICollection<PrivateMessage> Receivers { get; set; }

        public virtual ICollection<UnreadPrivateMessage> UnreadReceivers { get; set; }

        public virtual ICollection<PrivateMessage> Senders { get; set; }

        public virtual ICollection<UnreadPrivateMessage> UnreadSenders { get; set; }

        public virtual ICollection<Friend> Friends1 { get; set; }

        public virtual ICollection<Friend> Friends2 { get; set; }

        public virtual ICollection<ChatRoom> ChatRooms { get; set; }

        public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }
    }
}
