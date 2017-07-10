using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace GreenChat.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        ApplicationDbContext Context { get; }
        UserManager<ApplicationUser> UserManager { get; }
        //RoleManager<ApplicationRole> RoleManager { get; }
        IFriendRepository Friends { get; }
        IChatMessageRepository ChatMessages { get; }
        IUnreadChatMessageRepository UnreadChatMessages { get; }
        IPrivateMessageRepository PrivateMessages { get; }
        IUnreadPrivateMessageRepository UnreadPrivateMessages { get; }
        IChatRoomRepository ChatRooms { get; }
        IChatRoomUserRepository ChatRoomUsers { get;}

        Task SaveAsync();
    }
}
