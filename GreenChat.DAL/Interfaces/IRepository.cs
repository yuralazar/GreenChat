using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GreenChat.DAL.Interfaces
{
    public interface IRepository<T>: IDisposable where T:class
    {
        IQueryable<T> GetAll();
        Task<T> Get(int id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task Create(T item);
        void Update(T item);
        Task Delete(int id);
        void Delete(T item);
        Task SaveChages();
    }
}
