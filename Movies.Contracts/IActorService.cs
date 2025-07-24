using Movies.Core.Domain.Models.DTOs.ActorDtos;

namespace Movies.Contracts
{
    public interface IActorService : IBaseService<ActorDto, ActorCreateDto, ActorPutUpdateDto> { }
}
