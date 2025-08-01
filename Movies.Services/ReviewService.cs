using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork uow;
        private const int maxReviewsInMovie = 10;
        private const int maxReviewsInOldMovie = 5;
        private const int oldMovieAgeLimit = 20;

        public ReviewService(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        public async Task<bool> AnyAsync(Guid id) => await uow.Reviews.AnyAsync(id);


        public async Task<(IEnumerable<ReviewDto>, PaginationMetadata)> GetAllAsync(BaseRequestParams requestParams)
        {
            var (reviews, paginationMetadata) = await uow.Reviews.GetAllReviewsAsync(trackChanges: false, requestParams);

            return (reviews.Select(MapToDto), paginationMetadata);
        }


        public async Task<ReviewDto?> GetAsync(Guid id)
        {
            var review = await uow.Reviews.GetReviewAsync(id, trackChanges: false);

            return review != null ? MapToDto(review) : null;
        }

        public async Task<bool> IsMaxReviews(Guid movieId)
        {
            int movieYear = await uow.Movies.GetMovieYearAsync(movieId, trackChanges: false);
            int movieAge = DateTime.Now.Year - movieYear;

            var reviewsInMovie = await uow.Reviews.GetTotalReviewsInMovieAsync(movieId);

            return oldMovieAgeLimit > movieAge ?
                maxReviewsInMovie > reviewsInMovie ? false : true
                :
                maxReviewsInOldMovie > reviewsInMovie ? false : true;
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
            var review = await uow.Reviews.GetReviewAsync(id, trackChanges: true);
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
