using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Core.Domain.Contracts
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<(IEnumerable<Movie>, PaginationMetadata)> GetAllMoviesAsync(bool trackChanges, BaseRequestParams requestParams);
        Task<Movie?> GetMovieAsync(Guid id, bool trackChanges);
        Task<Movie?> GetMovieDetailedAsync(Guid id, bool trackChanges = false);
        Task<int> GetMovieYear(Guid id, bool trackChanges = false)
    }
}
