using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Data.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }
    }
}
