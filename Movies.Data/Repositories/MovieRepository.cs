using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Data.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }


        public async Task<(IEnumerable<Movie>, PaginationMetadata)> GetAllMoviesAsync(bool trackChanges, BaseRequestParams requestParams)
        {
            var query = FindAll(trackChanges);

            var totalItemCount = await query.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, requestParams.PageSize, requestParams.Page);

            query = query.Include(m => m.MovieDetails)
                         .Include(m => m.Genre)
                         .OrderBy(m => m.Title)
                         .Skip(requestParams.PageSize * (requestParams.Page - 1))
                         .Take(requestParams.PageSize);

            var collectionToReturn = await query.ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<Movie?> GetMovieAsync(Guid id, bool trackChanges)
        {
            return await FindAll(trackChanges).FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<Movie?> GetMovieDetailedAsync(Guid id, bool trackChanges = false)
        {
            return await FindAll(trackChanges)
                .Include(m => m.MovieDetails)
                .Include(m => m.Genre)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
