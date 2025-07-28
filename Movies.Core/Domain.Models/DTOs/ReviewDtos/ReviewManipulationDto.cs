using System.ComponentModel.DataAnnotations;

namespace Movies.Core.Domain.Models.DTOs.ReviewDtos
{
    public class GenreManipulationDto
    {
        [Required]
        public string ReviewerName { get; set; } = null!;
        [Required]
        public string Comment { get; set; } = null!;
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
