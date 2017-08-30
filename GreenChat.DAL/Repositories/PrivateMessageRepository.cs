using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using GreenChat.Data.Instances;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public struct PrivateMessageInfo
    {
        public PrivateMessage Message;
        public MessStatus Status;
    }

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

        public async Task<List<PrivateMessageInfo>> GetMessagesPortionBeforeDate(ApplicationUser sernder, ApplicationUser reciever, int count, DateTimeOffset date)
        {
            return await GetMessagesPortionBeforeDate(sernder.Id, reciever.Id, count, date);
        }

        public async Task<List<PrivateMessageInfo>> GetMessagesPortionBeforeDate(string senderId, string recieverId, int count, DateTimeOffset date)
        {
            
            var messages = await Context.PrivateMessages
                .Where(mess => ((mess.SenderID == senderId && mess.ReceiverID == recieverId) ||
                                (mess.SenderID == recieverId && mess.ReceiverID == senderId))
                                 && mess.Date < date)                
                .Include(message => message.Sender)
                .Include(message => message.Receiver)
                .OrderByDescending(message => message.Date)
                .Take(count)
                .ToListAsync();

            var messStatuses = from statuses1 in
                         Context.PrivateMessageStatuses
                           where statuses1.UserId == senderId
                           join lastStatuses in
                           (from statuses0 in Context.PrivateMessageStatuses
                            where statuses0.UserId == senderId
                            group statuses0 by statuses0.PrivateMessageId
                               into statusesGroup
                            select new
                            {
                                PrivateMessageId = statusesGroup.Key,
                                Date = (from stat in statusesGroup
                                        select stat.Date).Max()
                            }
                           )
                           on new { statuses1.PrivateMessageId, statuses1.Date }
                              equals
                              new { lastStatuses.PrivateMessageId, lastStatuses.Date }
                           select new { statuses1.Status, statuses1.PrivateMessageId };

            var list = messages.GroupJoin(messStatuses,
                    message => message.PrivateMessageID,
                    arg => arg.PrivateMessageId,
                    (message, statuses) => new {message, statuses}
                )
                .SelectMany(
                    temp => temp.statuses.DefaultIfEmpty(),
                    (temp, status) => new {temp.message, Status = status?.Status ?? MessStatus.Seen }
                )
                .OrderBy(row => row.message.Date)
                .Select(row => new PrivateMessageInfo{Message = row.message, Status = row.Status});                                              

            return list.ToList();
        }

        public async Task<List<PrivateMessage>> GetNotSeen(string userId)
        {
            var list = await 
             Context.PrivateMessageStatuses
            .Where(statuses => statuses.UserId == userId && statuses.Status != MessStatus.Seen)
            .Join(Context.PrivateMessageStatuses
                    .Where(statuses => statuses.UserId == userId)
                    .GroupBy(statuses => statuses.PrivateMessageId)
                    .Select(statusesGroup => new
                        {
                            PrivateMessageId = statusesGroup.Key,
                            Date = statusesGroup.Select(stat => stat.Date).Max()
                        })
                    ,
                    statuses => new {statuses.PrivateMessageId, statuses.Date},
                    lastStatuses => new {lastStatuses.PrivateMessageId, lastStatuses.Date},
                    (statuses, lastStatuses) => new {statuses.Status, statuses.PrivateMessage}
             )
            .Select(statuses => new {statuses.PrivateMessage, statuses.PrivateMessage.Receiver, statuses.PrivateMessage.Sender})            
            .ToListAsync();                          

            return list.Select(arg => arg.PrivateMessage).ToList();

        }
    }
}

