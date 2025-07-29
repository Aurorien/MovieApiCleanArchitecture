using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Data.Repositories
{
    internal class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context) { }


        public async Task<(IEnumerable<Genre>, PaginationMetadata)> GetAllGenresAsync(bool trackChanges, BaseRequestParams requestParams)
        {
            var query = FindAll(trackChanges);

            var totalItemCount = await query.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, requestParams.PageSize, requestParams.Page);

            query = query.Skip(requestParams.PageSize * (requestParams.Page - 1))
                         .Take(requestParams.PageSize);

            var collectionToReturn = await query.ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<Genre?> GetGenreAsync(Guid id, bool trackChanges, bool includeMovies)
        {
            return await FindAll(trackChanges).Include(m => m.Movies).FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var query = FindAll(trackChanges: false)
                .Where(g => g.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(g => g.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
