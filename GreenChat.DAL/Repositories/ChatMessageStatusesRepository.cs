using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class ChatMessageStatusesRepository : BaseRepository<ChatMessageStatus>, IChatMessageStatusesRepository
    {
        public ChatMessageStatusesRepository(ApplicationDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            DbSet = Context.ChatMessageStatuses;
        }

        public Task AddSentStatus(int messId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task AddDeliveredStatus(int messId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task AddSeenStatus(int messId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<MessStatus> GetStatus(int messId)
        {
            throw new NotImplementedException();
        }
    }
}