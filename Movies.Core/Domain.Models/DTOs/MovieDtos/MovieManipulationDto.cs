using System.ComponentModel.DataAnnotations;

namespace Movies.Core.Domain.Models.DTOs.MovieDtos
{
    public class MovieManipulationDto
    {
        [Required(ErrorMessage = "Movie Title is a required field")]

        [StringLength(200)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Movie Year is a required field")]
        [Range(1878, 2100)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Movie GenreId is a required field")]
        public Guid GenreId { get; set; }

        [Required(ErrorMessage = "Movie DurationInMinutes is a required field")]
        [Range(1, 55000)]
        public int DurationInMinutes { get; set; }

        [Required(ErrorMessage = "Movie Synopsis is a required field")]
        [StringLength(2000)]
        public string Synopsis { get; set; } = null!;

        [Required(ErrorMessage = "Movie Language is a required field")]
        [StringLength(50)]
        public string Language { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int? Budget { get; set; }
    }
}
