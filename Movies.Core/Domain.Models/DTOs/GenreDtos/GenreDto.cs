using System.Text.Json.Serialization;

namespace Movies.Core.Domain.Models.DTOs.GenreDtos
{
    public class GenreDto
    {
        [JsonPropertyOrder(1)] // attribute JsonPropertyOrder is used to decide the order of fields in the json object
        public Guid Id { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; } = null!;
    }
}
