using Movies.Core.Domain.Contracts;

namespace Movies.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        private readonly Lazy<IMovieRepository> movieRepository;
        private readonly Lazy<IActorRepository> actorRepository;
        private readonly Lazy<IReviewRepository> reviewRepository;

        public IMovieRepository Movies => movieRepository.Value;
        public IActorRepository Actors => actorRepository.Value;
        public IReviewRepository Reviews => reviewRepository.Value;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;

            movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(context));
            actorRepository = new Lazy<IActorRepository>(() => new ActorRepository(context));
            reviewRepository = new Lazy<IReviewRepository>(() => new ReviewRepository(context));
        }

        public async Task CompleteAsync() => await context.SaveChangesAsync();
    }
}
