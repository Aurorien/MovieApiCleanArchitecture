using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Core.Domain.Contracts
{
    public interface IActorRepository : IBaseRepository<Actor>
    {
        Task<(IEnumerable<Actor>, PaginationMetadata)> GetAllActorsAsync(bool trackChanges, BaseRequestParams requestParams);
        Task<Actor?> GetActorAsync(Guid id, bool trackChanges);
        Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId);
        Task<int> NumberOfActorsInMovieAsync(Guid movieId);
        void AddActorToMovie(MovieActor movieActor);
        Task RemoveActorFromMovieAsync(Guid movieId, Guid actorId);
    }
}
