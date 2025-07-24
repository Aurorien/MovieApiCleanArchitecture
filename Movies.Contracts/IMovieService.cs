using Movies.Core.Domain.Models.DTOs.MovieDtos;

namespace Movies.Contracts
{
    public interface IMovieService : IBaseService<MovieDto, MovieCreateDto, MoviePutUpdateDto>
    {
        Task<MovieDetailedDto?> GetDetailedAsync(Guid id, bool trackChanges = false);
    }
}
