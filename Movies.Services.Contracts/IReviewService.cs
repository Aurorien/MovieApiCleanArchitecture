using Movies.Core.Domain.Models.DTOs.ReviewDtos;

namespace Movies.Services.Contracts
{
    public interface IReviewService : IBaseService<ReviewDto, ReviewCreateDto, ReviewUpdateDto>
    {
        Task<ReviewDto?> GetAsync(Guid id);
        Task<bool> IsMaxReviews(Guid movieId);
    }
}
