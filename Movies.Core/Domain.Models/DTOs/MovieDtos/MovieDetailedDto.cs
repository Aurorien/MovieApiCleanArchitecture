using Movies.Core.Domain.Models.DTOs.ActorDtos;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;

namespace Movies.Core.Domain.Models.DTOs.MovieDtos
{
    public class MovieDetailedDto
    {
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int Budget { get; set; }
        public IEnumerable<ActorDto> Actors { get; set; } = Enumerable.Empty<ActorDto>();
        public IEnumerable<ReviewDto> Reviews { get; set; } = Enumerable.Empty<ReviewDto>();
    }
}
