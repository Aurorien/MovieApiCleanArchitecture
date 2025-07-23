using System.Linq.Expressions;

namespace Movies.Core.Domain.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false, params Expression<Func<T, object>>[] includes);
        Task<T?> GetAsync(Guid id, bool trackChanges = false, params Expression<Func<T, object>>[] includes);
        Task<bool> AnyAsync(Guid id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
