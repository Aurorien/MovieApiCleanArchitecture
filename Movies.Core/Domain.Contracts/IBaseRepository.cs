namespace Movies.Core.Domain.Contracts
{
    public interface IBaseRepository<T>
    {
        Task<bool> AnyAsync(Guid id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
