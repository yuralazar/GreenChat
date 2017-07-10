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
    public class PrivateMessageStatusesRepository : BaseRepository<PrivateMessageStatus>, IPrivateMessageStatusesRepository
    {
        public PrivateMessageStatusesRepository(ApplicationDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            DbSet = Context.PrivateMessageStatuses;
        }       

        public async Task AddSentStatus(int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, messId, date);
        }

        public async Task AddDeliveredStatus(int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, messId, date);
        }

        public async Task AddSeenStatus(int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, messId, date);
        }

        public async Task<MessStatus> GetStatus(int messId)
        {
            var messStatuses = await Find(status => status.PrivateMessageId == messId)
                .OrderByDescending(status => status.Date)
                .ToListAsync();            

            return messStatuses.Count > 0 ? messStatuses[0].Status : MessStatus.Sent;
        }

        private async Task AddStatus(MessStatus status, int messId, DateTime date)
        {
            var privateStatus = new PrivateMessageStatus
            {
                Date = date,
                PrivateMessageId = messId,
                Status = status
            };

            await Create(privateStatus);
            await SaveChages();
        }
    }
}