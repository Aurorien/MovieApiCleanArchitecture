using Movies.Core.DomainContracts;
using Movies.Core.Models.Entities;

namespace Movies.Data.Repositories
{
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }
    }
}
