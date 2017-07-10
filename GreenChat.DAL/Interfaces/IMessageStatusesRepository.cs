using System;
using System.Threading.Tasks;
using GreenChat.Data.Instances;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IMessageStatusesRepository<T> : IRepository<T> where T : class
    {
        Task AddSentStatus(int messId, DateTime date);
        Task AddDeliveredStatus(int messId, DateTime date);
        Task AddSeenStatus(int messId, DateTime date);
        Task<MessStatus> GetStatus(int messId);
    }
}