using System.Text.Json.Serialization;

namespace Movies.Core.Domain.Models.DTOs.GenreDtos
{
    public class GenreDto
    {
        [JsonPropertyOrder(1)]
        public Guid Id { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; } = null!;
    }
}
