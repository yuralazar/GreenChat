using System;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IMessageStatusesRepository<T> : IRepository<T> where T : class
    {
        Task AddSentStatus(string userId, int messId, DateTime date);
        Task AddDeliveredStatus(string userId, int messId, DateTime date);
        Task AddSeenStatus(string userId, int messId, DateTime date);
        Task AddStatus(MessStatus status, string userId, int messId, DateTime date);
        Task<MessStatus> GetStatus(string userId, int messId);
    }
}