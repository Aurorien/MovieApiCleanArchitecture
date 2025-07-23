using Movies.Core.Domain.Models.DTOs.MovieDtos;

namespace Movies.Core.Domain.Models.DTOs.ActorDtos
{
    public class ActorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public int BirthYear { get; set; }
        public IEnumerable<MovieTitlesDto> MovieTitles { get; set; } = Enumerable.Empty<MovieTitlesDto>();
    }
}
