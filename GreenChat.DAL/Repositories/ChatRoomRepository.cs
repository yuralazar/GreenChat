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
        }

        public override IQueryable<ChatRoom> GetAll()
        {
            return Context.ChatRooms;
        }

        public override Task<ChatRoom> Get(int id)
        {
            return Context.ChatRooms.FindAsync(id);            
        }

        public override IQueryable<ChatRoom> Find(Expression<Func<ChatRoom, bool>> predicate)
        {
            return Context.ChatRooms.Where(predicate);
        }

        public override async Task Create(ChatRoom item)
        {
            await Context.ChatRooms.AddAsync(item);
            await SaveChages();
        }

        public override void Update(ChatRoom item)
        {
            Context.Entry(item).State = EntityState.Modified;
            SaveChages();
        }

        public override async Task Delete(int id)
        {
            var chatRoom = await Context.ChatRooms.FindAsync(id);
            Context.ChatRooms.Remove(chatRoom);
        }
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        public override void Delete(ChatRoom chatRoom)
        {
            Context.ChatRooms.Remove(chatRoom);
            SaveChages();
        }        

    }
}
