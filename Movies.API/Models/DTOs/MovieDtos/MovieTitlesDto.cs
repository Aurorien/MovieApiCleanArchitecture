namespace Movies.API.Models.DTOs.MovieDtos
{
    public class MovieTitlesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
