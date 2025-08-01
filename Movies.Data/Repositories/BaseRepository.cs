using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;

namespace Movies.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext Context { get; }
        protected DbSet<T> DbSet { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        protected IQueryable<T> FindAll(bool trackChanges = false) => trackChanges ? DbSet : DbSet.AsNoTracking();

        public void Create(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
