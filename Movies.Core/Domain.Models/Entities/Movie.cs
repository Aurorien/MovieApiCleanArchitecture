namespace Movies.Core.Domain.Models.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public MovieDetails MovieDetails { get; set; } = null!; // Navigation property
        public ICollection<Review> Reviews { get; set; } = new List<Review>(); // 1:N - Movie:Review
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
