namespace Movies.API.Models.DTOs.ActorDtos
{
    public class MovieActorCreateDto
    {
        public Guid ActorId { get; set; }
        public string Role { get; set; } = null!;
    }
}