using Movies.Contracts;

namespace Movies.Services
{
    public class ServiceManager : IServiceManager
    {
        private Lazy<IMovieService> movieService;
        private Lazy<IActorService> actorService;
        private Lazy<IReviewService> reviewService;

        public IMovieService MovieService => movieService.Value;
        public IActorService ActorService => actorService.Value;
        public IReviewService ReviewService => reviewService.Value;

        public ServiceManager(Lazy<IMovieService> movieService, Lazy<IActorService> actorService, Lazy<IReviewService> reviewService)
        {
            this.movieService = movieService;
            this.actorService = actorService;
            this.reviewService = reviewService;
        }
    }

}
