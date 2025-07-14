using System.ComponentModel.DataAnnotations;

namespace Movies.API.Models.DTOs.ReviewDtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        [Required]
        public string ReviewerName { get; set; } = null!;
        [Required]
        public string Comment { get; set; } = null!;
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
