using System.Text.Json.Serialization;

namespace Movies.Core.Domain.Models.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }

        public Guid MovieId { get; set; } // Foreign key. 1:N - Movie:Review

        [JsonIgnore] // Prevents circular reference recursion
        public Movie Movie { get; set; } = null!; // Navigation property
    }
}
