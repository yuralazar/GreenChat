using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class ChatRoomUsersRepository: BaseRepository<ChatRoomUser>, IChatRoomUserRepository
    {
        public ChatRoomUsersRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
        }

        public override IQueryable<ChatRoomUser> GetAll()
        {
            return Context.ChatRoomUsers;
        }

        public override Task<ChatRoomUser> Get(int id)
        {
            return Context.ChatRoomUsers.FindAsync(id);
        }

        public override IQueryable<ChatRoomUser> Find(Expression<Func<ChatRoomUser, bool>> predicate)
        {
            return Context.ChatRoomUsers.Where(predicate);
        }

        public override async Task Create(ChatRoomUser item)
        {
            await Context.ChatRoomUsers.AddAsync(item);
            await SaveChages();
        }

        public override void Update(ChatRoomUser item)
        {
            Context.Entry(item).State = EntityState.Modified;
            SaveChages();
        }

        public override async Task Delete(int id)
        {
            var chatRoomUser = await Context.ChatRoomUsers.FindAsync(id);
            Context.ChatRoomUsers.Remove(chatRoomUser);
            await SaveChages();
        }
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task AddChatRoomUsers(List<ApplicationUser> users, int chatRoomId)
        {
            var list = new List<ChatRoomUser>();
            users.ForEach(user => list.Add(
                new ChatRoomUser
                {
                    ChatRoomID = chatRoomId,
                    UserID = user.Id
                }));

            await Context.ChatRoomUsers.AddRangeAsync(list);
            await SaveChages();
        }

        public async Task<List<ApplicationUser>> GetChatRoomUsers(int chatRoomId)
        {
            var query = await Context.ChatRoomUsers
                .Select(chatUser => new { chatUser.User, chatUser.ChatRoomID })                
                .Where(row => row.ChatRoomID == chatRoomId)
                .ToListAsync();

            var list = query
                .Select(row => row.User)
                .Distinct()
                .ToList();

            return list;
        }

        public async Task<ChatRoomUser> GetChatRoomUser(int chatRoomId, ApplicationUser user)
        {
            return await Context.ChatRoomUsers.FirstOrDefaultAsync(
                roomUser => roomUser.ChatRoomID == chatRoomId && roomUser.UserID == user.Id);
        }

        public async Task<List<ApplicationUser>> GetChatUsersList(int chatRoomId)
        {
            var query = await Context.ChatRoomUsers
                .Select(roomUser => new { roomUser.User, roomUser.ChatRoomID, roomUser.Confirmed })
                .Where(row => row.ChatRoomID == chatRoomId && row.Confirmed)                
                .ToListAsync();

            var list = query
                .Select(row => row.User)
                .Distinct()
                .ToList();

            return list;
        }

        public async Task ConfirmChatRoomUser(int chatRoomId, ApplicationUser user)
        {
            var charRoomUser =
                await Context.ChatRoomUsers.FirstOrDefaultAsync(
                    roomUser => roomUser.ChatRoomID == chatRoomId && roomUser.UserID == user.Id);
            charRoomUser.Confirmed = true;
            Update(charRoomUser);
        }

        public async Task RemoveChatRoomUser(int chatRoomId, ApplicationUser user)
        {
            var charRoomUser = await GetChatRoomUser(chatRoomId, user);
            Context.Remove(charRoomUser);
            await SaveChages();
        }

        public async Task<List<ChatRoomUser>> GetChatRoomsByUser(ApplicationUser user)
        {
            var query = await Context.ChatRoomUsers                
                .Where(row => row.UserID == user.Id)
                .Include(roomUser => roomUser.ChatRoom)
                .Include(roomUser => roomUser.User)
                .ToListAsync();

            return query;
        }

        public async Task<List<ChatRoomUser>> GetChatUsersList(List<ChatRoom> chatRooms)
        {
            var listIds = chatRooms.Select(room => room.ChatRoomID);

            var query = await Context.ChatRoomUsers                                
                .Where(row => listIds.Contains(row.ChatRoomID))
                .Include(user => user.User)
                .ToListAsync();

            return query;
        }

        public async Task AddChatRoomUser(ChatRoom сhatRoom, ApplicationUser userFrom)
        {
            await Create(new ChatRoomUser
            {
                ChatRoomID = сhatRoom.ChatRoomID,
                UserID =  userFrom.Id,
                Confirmed = true
            });
        }

        public override void Delete(ChatRoomUser item)
        {
            Context.ChatRoomUsers.Remove(item);
        }        
    }
}
