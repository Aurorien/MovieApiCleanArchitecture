using System.Text.Json.Serialization;

namespace Movies.Core.Domain.Models.Entities
{
    public class MovieDetails
    {
        public Guid Id { get; set; }
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int Budget { get; set; }
        public Guid MovieId { get; set; } // Foreign key. 1:1 - Movie:MovieDetails

        [JsonIgnore] // Prevents circular reference recursion
        public Movie Movie { get; set; } = null!; // Navigation property
    }
}
