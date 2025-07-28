using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Data.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }


        public async Task<(IEnumerable<Review>, PaginationMetadata)> GetAllReviewsAsync(bool trackChanges, BaseRequestParams requestParams)
        {
            var query = FindAll(trackChanges);

            var totalItemCount = await query.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, requestParams.PageSize, requestParams.Page);

            query = query.Skip(requestParams.PageSize * (requestParams.Page - 1))
                         .Take(requestParams.PageSize);

            var collectionToReturn = await query.ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<Review?> GetReviewAsync(Guid id, bool trackChanges)
        {
            return await FindAll(trackChanges).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
