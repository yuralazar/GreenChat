using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class ChatRoomRepository : BaseRepository<ChatRoom>, IChatRoomRepository
    {
        public ChatRoomRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
            DbSet = Context.ChatRooms;
        }

        public async Task<ChatRoom> AddChatRoom(ApplicationUser user, string name)
        {
            var chatRoom = new ChatRoom
            {
                Name = name,
                UserID = user.Id
            };
            await Create(chatRoom);                      
            return chatRoom;
        }

    }
}
