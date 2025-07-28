using Movies.Core.Domain.Models.DTOs.MovieDtos;
using System.Text.Json.Serialization;

namespace Movies.Core.Domain.Models.DTOs.GenreDtos
{
    public class GenreMoviesDto : GenreDto
    {
        [JsonPropertyOrder(10)]
        public IEnumerable<MovieDto> Movies { get; set; } = Enumerable.Empty<MovieDto>();
    }
}
