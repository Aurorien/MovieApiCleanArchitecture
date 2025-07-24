using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Data.Repositories
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext _context;

        public ActorRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
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
