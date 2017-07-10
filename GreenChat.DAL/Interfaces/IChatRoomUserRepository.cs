using System.Collections.Generic;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IChatRoomUserRepository : IRepository<ChatRoomUser>
    {
        Task AddChatRoomUsers(List<ApplicationUser> users, int chatRoomId);
        Task AddChatRoomUser(ChatRoom сhatRoom, ApplicationUser userFrom);
        Task<List<ApplicationUser>> GetChatRoomUsers(int chatRoomId);
        Task<ChatRoomUser> GetChatRoomUser(int chatRoomId, ApplicationUser user);
        Task<List<ApplicationUser>> GetChatUsersList(int chatRoomId);
        Task ConfirmChatRoomUser(int chatRoomId, ApplicationUser user);
        Task RemoveChatRoomUser(int chatRoomId, ApplicationUser user);
        Task<List<ChatRoomUser>> GetChatUsersList(List<ChatRoom> chatRooms);
        Task<List<ChatRoomUser>> GetChatRoomsByUser(ApplicationUser user);
    }
}