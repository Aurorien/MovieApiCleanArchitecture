using Movies.Contracts;

namespace Movies.Services
{
    public class ServiceManager : IServiceManager
    {
        private Lazy<IMovieService> movieService;
        private Lazy<IActorService> actorService;
        private Lazy<IReviewService> reviewService;
        private Lazy<IGenreService> genreService;

        public IMovieService MovieService => movieService.Value;
        public IActorService ActorService => actorService.Value;
        public IReviewService ReviewService => reviewService.Value;
        public IGenreService GenreService => genreService.Value;

        public ServiceManager(
                               Lazy<IMovieService> movieService,
                               Lazy<IActorService> actorService,
                               Lazy<IReviewService> reviewService,
                               Lazy<IGenreService> genreService
                             )
        {
            this.movieService = movieService;
            this.actorService = actorService;
            this.reviewService = reviewService;
            this.genreService = genreService;
        }
    }

}
