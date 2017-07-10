using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IUnreadChatMessageRepository : IRepository<UnreadChatMessage>
    {
        Task<List<UnreadChatMessage>> GetByChatRoomUser(string userId, int chatId);
        Task<List<UnreadChatMessage>> GetByChatRoomUser(string userId);
        Task Create(int chatMessageID, string userFromId, string userToId, int chatId, string content, DateTimeOffset date);
        Task Create(int chatMessageID, ApplicationUser userFrom, ApplicationUser userTo, int chatId, string content, DateTimeOffset date);
        Task DeleteByChatRoomUser(string userId, int chatId);        
    }
}
