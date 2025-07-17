namespace Movies.Core.DomainContracts
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task<bool> AnyAsync(Guid id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
