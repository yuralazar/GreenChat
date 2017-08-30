using System;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public abstract class BaseMessageStatusesRepository<TEntity> : BaseRepository<TEntity>, IMessageStatusesRepository<TEntity> where TEntity : class
    {
        protected BaseMessageStatusesRepository(ApplicationDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public abstract Task<MessStatus> GetStatus(string userId, int messId);
        public abstract Task AddStatus(MessStatus status, string userId, int messId, DateTime date);

        public async Task AddSentStatus(string userId, int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, userId, messId, date);
        }

        public async Task AddDeliveredStatus(string userId, int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, userId, messId, date);
        }

        public async Task AddSeenStatus(string userId, int messId, DateTime date)
        {
            await AddStatus(MessStatus.Sent, userId, messId, date);
        }
    }
}