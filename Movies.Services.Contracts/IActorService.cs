using Movies.Core.Domain.Models.DTOs.ActorDtos;

namespace Movies.Services.Contracts
{
    public interface IActorService : IBaseService<ActorDto, ActorCreateDto, ActorUpdateDto>
    {
        Task<ActorDto?> GetAsync(Guid id);
        Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId);
        Task<bool> AddActorToMovieAsync(Guid movieId, Guid actorId, string role);
    }
}
