using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenChat.DAL.Models;
using GreenChat.DAL.Repositories;

namespace GreenChat.DAL.Interfaces
{
    public interface IPrivateMessageRepository : IRepository<PrivateMessage>

    {
        Task<PrivateMessage> AddPrivateMessage(ApplicationUser sernder, ApplicationUser reciever, string content, DateTimeOffset date);
        Task<List<PrivateMessage>> GetMessagesPortionBeforeDate(ApplicationUser sernder, ApplicationUser reciever,
            int count, DateTimeOffset date);
        Task<List<PrivateMessage>> GetMessagesPortionBeforeDate(string sernderId, string recieverId,
            int count, DateTimeOffset date);       
    }
}