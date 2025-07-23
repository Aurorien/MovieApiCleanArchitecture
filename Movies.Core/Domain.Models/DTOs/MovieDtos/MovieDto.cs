using Movies.Core.Domain.Models.DTOs.ActorDtos;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;

namespace Movies.Core.Domain.Models.DTOs.MovieDtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int DurationInMinutes { get; set; }
        public string MovieDetailsLanguage { get; set; } = null!;
        public IEnumerable<ActorDto> Actors { get; set; } = Enumerable.Empty<ActorDto>();
        public IEnumerable<ReviewDto> Reviews { get; set; } = Enumerable.Empty<ReviewDto>();
    }
}
