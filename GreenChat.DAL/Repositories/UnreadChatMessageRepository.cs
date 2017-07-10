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
    public class UnreadChatMessageRepository : BaseRepository<UnreadChatMessage>, IUnreadChatMessageRepository
    {
        public UnreadChatMessageRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
        }

        public override IQueryable<UnreadChatMessage> GetAll()
        {
            return Context.UnreadChatMessages;
        }

        public override Task<UnreadChatMessage> Get(int id)
        {
            return Context.UnreadChatMessages.FindAsync(id);
        }

        public override IQueryable<UnreadChatMessage> Find(Expression<Func<UnreadChatMessage, bool>> predicate)
        {
            return Context.UnreadChatMessages.Where(predicate);
        }

        public override async Task Create(UnreadChatMessage item)
        {
            await Context.UnreadChatMessages.AddAsync(item);
            await SaveChages();
        }

        public override void Update(UnreadChatMessage item)
        {
            Context.Entry(item).State = EntityState.Modified;
            SaveChages();
        }

        public override async Task Delete(int id)
        {
            var chatMessage = await Context.UnreadChatMessages.FindAsync(id);
            Context.UnreadChatMessages.Remove(chatMessage);
            await SaveChages();
        }        

        public async Task Create(int chatMessageID, string userFromId, string userToId, int chatId, string content, DateTimeOffset date)
        {
            var chUserFrom = await Context.ChatRoomUsers
                .FirstOrDefaultAsync(chatUser => chatUser.UserID == userFromId && chatUser.ChatRoomID == chatId);
            var chUserTo = await Context.ChatRoomUsers
                .FirstOrDefaultAsync(chatUser => chatUser.UserID == userToId && chatUser.ChatRoomID == chatId);
            var chatMessage = new UnreadChatMessage
            {                
                ChatMessageID = chatMessageID,
                Content = content,
                Date = date,
                ChatRoomUserFromID = chUserFrom.ChatRoomUserID,
                ChatRoomUserToID = chUserTo.ChatRoomUserID,
                ChatRoomID = chatId

            };
            await Create(chatMessage);
        }

        public async Task Create(int chatMessageID, ApplicationUser userFrom, ApplicationUser userTo, int chatId, string content, DateTimeOffset date)
        {
            await Create(chatMessageID, userFrom.Id, userTo.Id, chatId, content, date);
        }

        public async Task<List<UnreadChatMessage>> GetByChatRoomUser(string userId, int chatId)
        {
            var list = await Find(unread => unread.ChatRoomID == chatId &&
                                unread.ChatRoomUserTo.UserID == userId &&
                                unread.ChatRoomUserTo.ChatRoomID == chatId)
                .Include(unread => unread.ChatRoomUserFrom)
                    .ThenInclude(user => user.User)
                .Include(unread => unread.ChatRoomUserTo)
                    .ThenInclude(user => user.User)
                .Include(unread => unread.ChatRoom)                
                .ToListAsync();
            return list;
        }

        public async Task<List<UnreadChatMessage>> GetByChatRoomUser(string userId)
        {
            var list = await Find(unread => unread.ChatRoomUserTo.UserID == userId)
                .Include(unread => unread.ChatRoomUserFrom)
                    .ThenInclude(user => user.User)
                .Include(unread => unread.ChatRoomUserTo)
                    .ThenInclude(user => user.User) 
                .Include(unread => unread.ChatRoom)                
                .ToListAsync();
            return list;
        }

        public override void Delete(UnreadChatMessage chatMessage)
        {
            Context.UnreadChatMessages.Remove(chatMessage);
            SaveChages();
        }

        public async Task DeleteByChatRoomUser(string userId, int chatId)
        {
            var list = await GetByChatRoomUser(userId, chatId);
            list.ForEach(message => Context.UnreadChatMessages.Remove(message));
            await SaveChages();
        }

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

        private bool _disposed = false;
    }
}
