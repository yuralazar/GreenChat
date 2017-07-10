using System;
using System.Collections.Generic;
using System.Text;
using GreenChat.DAL.Interfaces;
using GreenChat.DAL.Models;
using GreenChat.DAL.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public class FriendRepository : BaseRepository<Friend>, IFriendRepository
    {
        public FriendRepository(ApplicationDbContext context, ILoggerFactory factory) : base(context, factory)
        {
            DbSet = Context.Friends;
        }
        
        public async Task<List<ApplicationUser>> GetPotentialFriends(ApplicationUser user)
        {
            var query =
                from fr1 in Context.Friends
                where fr1.Friend2 == user
                join fr2 in Context.Friends
                on new { Id1 = fr1.Friend1ID, Id2 = fr1.Friend2ID }
                equals
                new { Id1 = fr2.Friend2ID, Id2 = fr2.Friend1ID }
                into leftFr
                from fr in leftFr.DefaultIfEmpty()
                where fr == null
                select fr1.Friend1;

            return await query.ToListAsync();
        }

        public async Task AddFriend(ApplicationUser userFrom, ApplicationUser userTo)
        {
            await Create(new Friend
            {
                Friend1ID = userFrom.Id,
                Friend2ID = userTo.Id
            });
        }

        public async Task Delete(ApplicationUser userFrom, ApplicationUser userTo)
        {
            var friends = await Find(friend => friend.Friend1ID == userFrom.Id && friend.Friend2ID == userTo.Id).ToListAsync();
            friends.ForEach(Delete);
        }

        public async Task<List<ApplicationUser>> GetFriends(ApplicationUser user)
        {
            var query = from fr1 in Context.Friends
                where fr1.Friend1 == user
                join fr2 in Context.Friends
                on new { Id1 = fr1.Friend1ID, Id2 = fr1.Friend2ID }
                equals
                new { Id1 = fr2.Friend2ID, Id2 = fr2.Friend1ID }
                select fr1.Friend2;

            return await query.ToListAsync();
        }

    }
}
