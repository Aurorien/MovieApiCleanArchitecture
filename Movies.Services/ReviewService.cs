using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using System.Linq.Expressions;

namespace Movies.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork uow;

        public ReviewService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> AnyAsync(Guid id) => await uow.Reviews.AnyAsync(id);
        public async Task<IEnumerable<Review>> GetAllAsync(bool trackChanges = false, params Expression<Func<Review, object>>[] includes)
            => await uow.Reviews.GetAllAsync(trackChanges, includes);

        public async Task<Review?> GetAsync(Guid id, bool trackChanges = false, params Expression<Func<Review, object>>[] includes)
            => await uow.Reviews.GetAsync(id, trackChanges, includes);

        public void Create(Review review) => uow.Reviews.Create(review);
        public void Update(Review review) => uow.Reviews.Update(review);
        public void Delete(Review review) => uow.Reviews.Delete(review);
    }
}
