using System.Collections.Generic;
using System.Threading.Tasks;
using GreenChat.DAL.Models;

namespace GreenChat.DAL.Interfaces
{
    public interface IFriendRepository : IRepository<Friend>
    {
        Task<List<ApplicationUser>> GetPotentialFriends(ApplicationUser user);
        Task AddFriend(ApplicationUser userFrom, ApplicationUser userTo);
        Task<List<ApplicationUser>> GetFriends(ApplicationUser user);
        Task Delete(ApplicationUser userTo, ApplicationUser userFrom);
    }
}