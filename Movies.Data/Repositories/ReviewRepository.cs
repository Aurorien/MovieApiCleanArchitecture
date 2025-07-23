using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Data.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }
    }
}
