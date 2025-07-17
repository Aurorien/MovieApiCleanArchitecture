using Movies.Core.DomainContracts;
using Movies.Core.Models.Entities;

namespace Movies.Data.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }
    }
}
