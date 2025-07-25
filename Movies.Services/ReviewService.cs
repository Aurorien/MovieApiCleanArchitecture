using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork uow;

        public ReviewService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> AnyAsync(Guid id) => await uow.Reviews.AnyAsync(id);

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await uow.Reviews.GetAllAsync(trackChanges: false);

            return reviews.Select(MapToDto);
        }

        public async Task<ReviewDto?> GetAsync(Guid id)
        {
            var review = await uow.Reviews.GetAsync(id, trackChanges: false);

            return review != null ? MapToDto(review) : null;
        }

        public async Task<ReviewDto> CreateAsync(ReviewCreateDto createDto)
        {
            var review = new Review
            {
                ReviewerName = createDto.ReviewerName,
                Comment = createDto.Comment,
                Rating = createDto.Rating
            };

            uow.Reviews.Create(review);
            await uow.CompleteAsync();

            return MapToDto(review);
        }

        public async Task<bool> UpdateAsync(Guid id, ReviewPutUpdateDto updateDto)
        {
            var review = await uow.Reviews.GetAsync(id, trackChanges: true);
            if (review == null)
            {
                return false;
            }

            review.ReviewerName = updateDto.ReviewerName;
            review.Comment = updateDto.Comment;
            review.Rating = updateDto.Rating;

            try
            {
                uow.Reviews.Update(review);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Reviews.AnyAsync(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await uow.Reviews.AnyAsync(id))
                return false;

            //Stub entity. No need to pass or get the whole entity for deletion, EF is only going to use the id of the entity to remove it anyway
            var review = new Review { Id = id };

            try
            {
                uow.Reviews.Delete(review);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Reviews.AnyAsync(id))
                    return false;

                throw;
            }
        }
        private ReviewDto MapToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating
            };
        }
    }
}
