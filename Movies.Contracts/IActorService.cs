using Movies.Core.Domain.Models.DTOs.ActorDtos;

namespace Movies.Contracts
{
    public interface IActorService : IBaseService<ActorDto, ActorCreateDto, ActorPutUpdateDto>
    {
        Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId);
        Task<bool> AddActorToMovieAsync(Guid movieId, Guid actorId, string role);
    }
}
