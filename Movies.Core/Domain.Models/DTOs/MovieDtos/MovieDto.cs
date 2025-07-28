namespace Movies.Core.Domain.Models.DTOs.MovieDtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int DurationInMinutes { get; set; }
    }
}
