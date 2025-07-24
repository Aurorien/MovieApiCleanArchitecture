using Movies.Core.Domain.Models.DTOs.ReviewDtos;

namespace Movies.Contracts
{
    public interface IReviewService : IBaseService<ReviewDto, ReviewCreateDto, ReviewPutUpdateDto> { }
}
