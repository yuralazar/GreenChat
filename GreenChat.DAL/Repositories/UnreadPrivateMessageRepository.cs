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
    public class UnreadPrivateMessageRepository : BaseRepository<UnreadPrivateMessage>, IUnreadPrivateMessageRepository
    {
        public UnreadPrivateMessageRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
        }

        public override IQueryable<UnreadPrivateMessage> GetAll()
        {
            return Context.UnreadPrivateMessages;
        }

        public override Task<UnreadPrivateMessage> Get(int id)
        {
            return Context.UnreadPrivateMessages.FindAsync(id);
        }

        public override IQueryable<UnreadPrivateMessage> Find(Expression<Func<UnreadPrivateMessage, bool>> predicate)
        {
            return Context.UnreadPrivateMessages.Where(predicate);
        }

        public override async Task Create(UnreadPrivateMessage item)
        {
            await Context.UnreadPrivateMessages.AddAsync(item);
            await SaveChages();
        }

        public override void Update(UnreadPrivateMessage item)
        {
            Context.Entry(item).State = EntityState.Modified;
            SaveChages();
        }

        public override async Task Delete(int id)
        {
            var unreadPrivateMessage = await Context.UnreadPrivateMessages.FindAsync(id);
            Context.UnreadPrivateMessages.Remove(unreadPrivateMessage);
            await SaveChages();
        }

        public override void Delete(UnreadPrivateMessage unreadPrivateMessage)
        {            
            Context.UnreadPrivateMessages.Remove(unreadPrivateMessage);
            SaveChages();
        }

        public async Task Create(int privateMessageID, ApplicationUser sernder, ApplicationUser reciever, string content, DateTimeOffset date)
        {
            var unreadPrivateMessage = new UnreadPrivateMessage
            {
                PrivateMessageID = privateMessageID,
                Content = content,
                SenderID = sernder.Id,
                ReceiverID = reciever.Id,
                Date = date
            };
            await Create(unreadPrivateMessage);
        }

        public async Task<List<UnreadPrivateMessage>> Get(string senderId, string recieverId)
        {
            var list = await Find(unread => unread.SenderID == senderId && unread.ReceiverID == recieverId).ToListAsync();
            return list;
        }

        public async Task<List<UnreadPrivateMessage>> Get(string recieverId)
        {
            var list = await Find(unread => unread.ReceiverID == recieverId)
                .Include(message => message.Sender)
                .Include(message => message.Receiver)                
                .ToListAsync();
            return list;
        }

        public async Task Delete(string senderId, string recieverId)
        {
            var list = await Get(senderId, recieverId);
            list.ForEach(message => Context.UnreadPrivateMessages.Remove(message));            
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
    }
}

