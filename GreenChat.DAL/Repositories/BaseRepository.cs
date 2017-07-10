using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace GreenChat.DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext Context;
        public ILogger Logger;

        protected BaseRepository(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            Context = context;
            Logger = loggerFactory.CreateLogger<BaseRepository<TEntity>>();
        }

        public async Task SaveChages()
        {
            await Context.SaveChangesAsync();         
        }

        public abstract void Dispose();
        public abstract IQueryable<TEntity> GetAll();
        public abstract Task<TEntity> Get(int id);
        public abstract IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        public abstract Task Create(TEntity item);
        public abstract void Update(TEntity item);
        public abstract Task Delete(int id);
        public abstract void Delete(TEntity item);
    }
}
