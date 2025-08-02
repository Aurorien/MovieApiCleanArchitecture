using Movies.Core.Domain.Models.DTOs.MovieDtos;

namespace Movies.Contracts
{
    public interface IMovieService : IBaseService<MovieDto, MovieCreateDto, MovieUpdateDto>
    {
        Task<MovieDto?> GetAsync(Guid id);
        Task<MovieDetailedDto?> GetDetailedAsync(Guid id);
        Task<bool> IsMovieDocumentaryAsync(Guid movieId);
        Task<bool> IsGenreIdDocumentaryAsync(Guid genreId);
        Task<bool> IsDocumentaryActorLimitReachedAsync(Guid movieId);
        Task<bool> IsDocumentaryBudgetLimitReachedAsync(Guid genreId, int? newBudget);
        Task<MovieUpdateDto> GetUpdateDtoAsync(Guid id);
    }
}
