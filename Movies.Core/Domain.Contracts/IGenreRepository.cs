using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Core.Domain.Contracts
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<(IEnumerable<Genre>, PaginationMetadata)> GetAllGenresAsync(bool trackChanges, BaseRequestParams requestParams);
        Task<Genre?> GetGenreAsync(Guid id, bool trackChanges, bool includeMovies);
    }
}