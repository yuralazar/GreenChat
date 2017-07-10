using System;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace GreenChat.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        //public RoleManager<ApplicationRole> RoleManager { get; }
        public IFriendRepository Friends { get; }
        public IChatMessageRepository ChatMessages { get; }
        public IPrivateMessageRepository PrivateMessages { get; }
        public IChatRoomRepository ChatRooms { get; }
        public IChatRoomUserRepository ChatRoomUsers { get; }
        public IPrivateMessageStatusesRepository PrivateMessageStatuses { get; }
        public IChatMessageStatusesRepository ChatMessageStatuses { get; }

        public UnitOfWork(
             ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            //, RoleManager<ApplicationRole> roleManager
            , IFriendRepository friendRepository
            , IChatMessageRepository chatMessageRepository
            , IChatRoomRepository chatRoomRepository
            , IChatRoomUserRepository chatRoomUsersRepository
            , IPrivateMessageRepository privateMessageRepository
            , IPrivateMessageStatusesRepository privateMessageStatuses 
            , IChatMessageStatusesRepository chatMessageStatuses)
        {
            Context = context;
            UserManager = userManager;
            //RoleManager = roleManager;
            Friends = friendRepository;
            ChatMessages = chatMessageRepository;
            ChatRooms = chatRoomRepository;
            ChatRoomUsers = chatRoomUsersRepository;
            PrivateMessages = privateMessageRepository;
            UnreadChatMessages = unreadChatMessages;
            UnreadPrivateMessages = unreadPrivateMessages;
            PrivateMessageStatuses = privateMessageStatuses;
            ChatMessageStatuses = chatMessageStatuses;
        }   

        public async Task SaveAsync()
        {
            //await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing)
            {
                Context.Dispose();
                UserManager.Dispose();                
                ChatMessages.Dispose();
                ChatRooms.Dispose();
                ChatRoomUsers.Dispose();
                Friends.Dispose();
                PrivateMessages.Dispose();
            }
            this._disposed = true;
        }
    }
}