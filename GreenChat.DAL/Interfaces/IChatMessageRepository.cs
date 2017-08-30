using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IChatMessageRepository : IRepository<ChatMessage>
    {
        Task<ChatMessage> AddChatMessage(ApplicationUser userFrom, int chatId, string content, DateTimeOffset date);        
        Task<List<ChatMessage>> GetMessagesPortionBeforeDate(int chatId, int count, DateTimeOffset date);
        Task<List<ChatMessage>> GetNotSeen(string userId);
    }
}
