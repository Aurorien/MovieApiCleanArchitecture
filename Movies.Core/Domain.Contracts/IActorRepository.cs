using Movies.Core.Domain.Models.Entities;

namespace Movies.Core.Domain.Contracts
{
    public interface IActorRepository : IBaseRepository<Actor>
    {
        Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId);
        void AddActorToMovie(MovieActor movieActor);
        Task RemoveActorFromMovieAsync(Guid movieId, Guid actorId);
    }
}
