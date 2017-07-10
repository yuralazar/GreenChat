using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GreenChat.DAL.Data;
using GreenChat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Clauses;

namespace GreenChat.DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected ApplicationDbContext Context;
        protected DbSet<TEntity> DbSet;
        protected bool Disposed;
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

        public void Dispose()
        {
            if (!Disposed)
            {
                Context.Dispose();
            }
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public async Task<TEntity> Get(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public async Task Create(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChages();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            SaveChages();
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);
            DbSet.Remove(entity);
            await SaveChages();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            SaveChages();
        }
    }
}
