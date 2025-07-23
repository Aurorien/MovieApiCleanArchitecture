using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using System.Linq.Expressions;

namespace Movies.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork uow;

        public MovieService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> AnyAsync(Guid id) => await uow.Movies.AnyAsync(id);
        public async Task<IEnumerable<Movie>> GetAllAsync(bool trackChanges = false, params Expression<Func<Movie, object>>[] includes)
            => await uow.Movies.GetAllAsync(trackChanges, includes);

        public async Task<Movie?> GetAsync(Guid id, bool trackChanges = false, params Expression<Func<Movie, object>>[] includes)
            => await uow.Movies.GetAsync(id, trackChanges, includes);

        public void Create(Movie movie) => uow.Movies.Create(movie);
        public void Update(Movie movie) => uow.Movies.Update(movie);
        public void Delete(Movie movie) => uow.Movies.Delete(movie);

    }
}
