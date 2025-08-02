using Movies.Core.Domain.Models.DTOs.GenreDtos;

namespace Movies.Services.Contracts
{
    public interface IGenreService : IBaseService<GenreDto, GenreCreateDto, GenreUpdateDto>
    {
        Task<GenreDto?> GetAsync(Guid id, bool includeMovies);
    }
}