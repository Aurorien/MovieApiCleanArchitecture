using Movies.Core.Domain.Models.DTOs.ReviewDtos;

namespace Movies.Contracts
{
    public interface IReviewService : IBaseService<ReviewDto, ReviewCreateDto, GenrePutUpdateDto>
    {
        Task<ReviewDto?> GetAsync(Guid id);
        Task<bool> IsMaxReviews(Guid movieId);
    }
}
