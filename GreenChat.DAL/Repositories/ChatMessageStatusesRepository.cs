using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class ChatMessageStatusesRepository : BaseMessageStatusesRepository<ChatMessageStatus>, IChatMessageStatusesRepository
    {
        public ChatMessageStatusesRepository(ApplicationDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            DbSet = Context.ChatMessageStatuses;
        }

        public override async Task<MessStatus> GetStatus(string userId, int messId)
        {
            var messStatuses = await Find(status => status.ChatMessageId == messId && status.UserId == userId)
                .OrderByDescending(status => status.Date)
                .ToListAsync();

            return messStatuses.Count > 0 ? messStatuses[0].Status : MessStatus.Sent;
        }

        public override async Task AddStatus(MessStatus status, string userId, int messId, DateTime date)
        {
            var privateStatus = new ChatMessageStatus
            {
                UserId = userId,
                Date = date,
                ChatMessageId = messId,
                Status = status
            };

            await Create(privateStatus);
            await SaveChages();
        }
    }
}