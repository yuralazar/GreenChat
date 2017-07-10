using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IChatRoomRepository : IRepository<ChatRoom>
    {
        Task<ChatRoom> AddChatRoom(ApplicationUser user, string name);
    }
}
