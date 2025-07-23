using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using System.Linq.Expressions;

namespace Movies.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class //ToDo: Entity base class
    {
        protected DbSet<T> DbSet { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            DbSet = context.Set<T>();
        }

        public async Task<bool> AnyAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false, params Expression<Func<T, object>>[] includes)
        {
            var query = trackChanges ? DbSet : DbSet.AsNoTracking();

            foreach (var include in includes)
            {
                if (include != null)
                    query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Guid id, bool trackChanges = false, params Expression<Func<T, object>>[] includes)
        {
            var query = trackChanges ? DbSet : DbSet.AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await DbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public void Create(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity) => DbSet.Remove(entity);
    }
}
