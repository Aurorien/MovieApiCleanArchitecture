using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Core.Domain.Contracts
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task<(IEnumerable<Review>, PaginationMetadata)> GetAllReviewsAsync(bool trackChanges, BaseRequestParams requestParams);
        Task<Review?> GetReviewAsync(Guid id, bool trackChanges);
    }
}
