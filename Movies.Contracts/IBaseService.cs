using System.Linq.Expressions;

namespace Movies.Contracts
{
    public interface IBaseService<T>
    {
        Task<bool> AnyAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false, params Expression<Func<T, object>>[] includes);

        Task<T?> GetAsync(Guid id, bool trackChanges = false, params Expression<Func<T, object>>[] includes);

        public void Create(T movie);
        public void Update(T movie);
        public void Delete(T movie);
    }
}
