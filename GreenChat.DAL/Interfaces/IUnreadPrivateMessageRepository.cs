using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IUnreadPrivateMessageRepository : IRepository<UnreadPrivateMessage>
    {
        Task Create(int privateMessageID, ApplicationUser sender, ApplicationUser reciever, string content, DateTimeOffset date);
        Task<List<UnreadPrivateMessage>> Get(string senderId, string recieverId);
        Task<List<UnreadPrivateMessage>> Get(string recieverId);
        Task Delete(string senderId, string recieverId);

    }
}