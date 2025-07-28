namespace Movies.Core.Domain.Models.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // 1:N - Genre:Movie
    }
}
