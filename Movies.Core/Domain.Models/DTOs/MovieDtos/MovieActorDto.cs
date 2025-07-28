namespace Movies.Core.Domain.Models.DTOs.MovieDtos
{
    public class MovieActorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public int BirthYear { get; set; }
        public string Role { get; set; } = null!;
    }
}
