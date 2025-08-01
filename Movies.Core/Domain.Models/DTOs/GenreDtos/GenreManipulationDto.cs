using System.ComponentModel.DataAnnotations;

namespace Movies.Core.Domain.Models.DTOs.GenreDtos
{
    public class GenreManipulationDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
