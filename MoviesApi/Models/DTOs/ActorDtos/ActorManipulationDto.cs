namespace MoviesApi.Models.DTOs.ActorDtos
{
    public class ActorManipulationDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int BirthYear { get; set; }
    }
}
