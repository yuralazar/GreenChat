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
    public class PrivateMessageRepository : BaseRepository<PrivateMessage>, IPrivateMessageRepository
    {
        public PrivateMessageRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
            DbSet = Context.PrivateMessages;
        }       

        public async Task<PrivateMessage> AddPrivateMessage(ApplicationUser sernder, ApplicationUser reciever, string content, DateTimeOffset date)
        {
            var privateMessage = new PrivateMessage
            {
                Content = content,
                SenderID = sernder.Id,
                ReceiverID = reciever.Id,
                Date = date
            };

            await Create(privateMessage);            
            return privateMessage;
        }

        public async Task<List<PrivateMessage>> GetMessagesPortionBeforeDate(ApplicationUser sernder, ApplicationUser reciever, int count, DateTimeOffset date)
        {
            return await GetMessagesPortionBeforeDate(sernder.Id, reciever.Id, count, date);
        }

        public async Task<List<PrivateMessage>> GetMessagesPortionBeforeDate(string sernderId, string recieverId, int count, DateTimeOffset date)
        {
            var list = await Find(mess => ((mess.SenderID == sernderId && mess.ReceiverID == recieverId) ||
                                           (mess.SenderID == recieverId && mess.ReceiverID == sernderId))
                                          && mess.Date < date)
                .Include(message => message.Sender)
                .Include(message => message.Receiver)
                .OrderByDescending(message => message.Date)
                .Take(count)
                .ToListAsync();                                

            return list.OrderBy(message => message.Date).ToList();
        }

        public override void Delete(PrivateMessage item)
        {
            Context.PrivateMessages.Remove(item);
            SaveChages();
        }        

    }
}

