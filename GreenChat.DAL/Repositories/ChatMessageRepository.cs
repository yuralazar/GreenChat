using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class ChatMessageRepository : BaseRepository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
            DbSet = Context.ChatMessages;
        }

        public async Task<ChatMessage> AddChatMessage(ApplicationUser userFrom, int chatId, string content, DateTimeOffset date)
        {
            var chUser = await Context.ChatRoomUsers
                .FirstOrDefaultAsync(chatUser => chatUser.UserID == userFrom.Id
                                                 && chatUser.ChatRoomID == chatId);
            var chatMessage = new ChatMessage
            {
                Content = content,
                Date = date,
                ChatRoomID = chatId,
                ChatRoomUserID = chUser.ChatRoomUserID
            };

            await Create(chatMessage);              
            return chatMessage;
        }

        public async Task<List<ChatMessage>> GetMessagesPortionBeforeDate(int chatId, int count, DateTimeOffset date)
        {
            var list = await Find(mess => (mess.ChatRoomID == chatId && mess.Date < date))
                .Include(message => message.ChatRoom)
                .Include(message => message.ChatRoomUser)
                .ThenInclude(user => user.User)
                .OrderByDescending(message => message.Date)
                .Take(count)
                .ToListAsync();

            return list.OrderBy(message => message.Date).ToList();
        }

        public async Task<List<ChatMessage>> GetNotSeen(string userId)
        {
            var list =
                from statuses in
                (from statuses1 in
                     Context.ChatMessageStatuses
                 where statuses1.UserId == userId
                 join lastStatuses in
                 (from statuses0 in Context.ChatMessageStatuses
                  where statuses0.UserId == userId
                  group statuses0 by statuses0.ChatMessageId
                     into statusesGroup
                  select new
                  {
                      ChatMessageId = statusesGroup.Key,
                      Date = (from stat in statusesGroup
                              select stat.Date).Max()
                  }
                 )
                 on new { statuses1.ChatMessageId, statuses1.Date } equals
                 new { lastStatuses.ChatMessageId, lastStatuses.Date }
                 select new { statuses1.Status, statuses1.ChatMessageId })
                where statuses.Status != MessStatus.Seen
                select statuses.ChatMessageId;


            return await Context.ChatMessages
                .Where(message => list.Contains(message.ChatMessageID))
                .Include(message => message.ChatRoom)
                .Include(message => message.ChatRoomUser)
                .ThenInclude(chatUser => chatUser.User)
                .ToListAsync();
        }
    }
}
