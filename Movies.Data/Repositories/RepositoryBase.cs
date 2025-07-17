using Microsoft.EntityFrameworkCore;
using Movies.Core.DomainContracts;

namespace Movies.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class //ToDo: Entity base class
    {
        protected DbSet<T> DbSet { get; }

        public RepositoryBase(ApplicationDbContext context)
        {
            DbSet = context.Set<T>();
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public void Create(T entity) => DbSet.Add(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
