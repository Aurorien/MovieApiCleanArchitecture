using System.ComponentModel.DataAnnotations;

namespace Movies.Core.Domain.Models.DTOs.GenreDtos
{
    public class GenreManipulationDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
