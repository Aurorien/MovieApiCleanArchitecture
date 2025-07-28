using Movies.Core.Domain.Models.DTOs.GenreDtos;

namespace Movies.Contracts
{
    public interface IGenreService : IBaseService<GenreDto, GenreCreateDto, GenrePutUpdateDto>
    {
        Task<GenreDto?> GetAsync(Guid id, bool includeMovies);
    }
}