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
        public int? Budget { get; set; }
        public IEnumerable<MovieActorDto> Actors { get; set; } = Enumerable.Empty<MovieActorDto>();
        public IEnumerable<ReviewDto> Reviews { get; set; } = Enumerable.Empty<ReviewDto>();
    }
}
