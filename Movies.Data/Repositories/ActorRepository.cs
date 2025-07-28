using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Data.Repositories
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext _context;


        public ActorRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }


        public async Task<(IEnumerable<Actor>, PaginationMetadata)> GetAllActorsAsync(bool trackChanges, BaseRequestParams requestParams)
        {
            var query = FindAll(trackChanges);

            var totalItemCount = await query.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, requestParams.PageSize, requestParams.Page);

            query = query.Include(a => a.MovieActors)
                            .ThenInclude(ma => ma.Movie)
                         .OrderBy(e => e.LastName)
                         .Skip(requestParams.PageSize * (requestParams.Page - 1))
                         .Take(requestParams.PageSize);

            var collectionToReturn = await query.ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<Actor?> GetActorAsync(Guid id, bool trackChanges)
        {
            return await FindAll(trackChanges)
                            .Include(a => a.MovieActors)
                                .ThenInclude(ma => ma.Movie)
                            .FirstOrDefaultAsync(a => a.Id == id);
        }


        public async Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId)
        {
            return await _context.Set<MovieActor>()
                                 .AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);
        }


        public void AddActorToMovie(MovieActor movieActor)
        {
            _context.Set<MovieActor>().Add(movieActor);
        }


        public async Task RemoveActorFromMovieAsync(Guid movieId, Guid actorId)
        {
            var movieActor = await _context.Set<MovieActor>()
                                           .FirstOrDefaultAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId);

            if (movieActor != null)
            {
                _context.Set<MovieActor>().Remove(movieActor);
            }
        }
    }
}
