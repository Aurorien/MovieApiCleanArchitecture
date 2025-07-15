namespace Movies.Core.Models.Entities
{
    public class MovieActor
    {
        public string Role { get; set; } = null!;

        public Guid MovieId { get; set; }
        public Guid ActorId { get; set; }
        public Movie Movie { get; set; } = null!;
        public Actor Actor { get; set; } = null!;
    }
}
