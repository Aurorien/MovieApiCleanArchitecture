using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Domain.Models.Entities;
using Movies.Data;

namespace Movies.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Reviews
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {

            var reviewDtos = await _context.Review
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
                .ToListAsync();


            return Ok(reviewDtos);
        }


        // GET: api/Reviews/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDto>> GetReview([FromRoute] Guid id)
        {
            var reviewDto = await _context.Review
                .Where(r => r.Id == id)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
                .FirstOrDefaultAsync();


            if (reviewDto == null)
            {
                return NotFound();
            }

            return Ok(reviewDto);
        }


        // POST: api/reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewDto>> PostReview([FromBody] ReviewCreateDto createReviewDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = new Review
            {
                ReviewerName = createReviewDto.ReviewerName,
                Comment = createReviewDto.Comment,
                Rating = createReviewDto.Rating
            };

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating
            };

            return CreatedAtAction("GetReview", new { id = reviewDto.Id }, reviewDto);
        }


        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutReview([FromRoute] Guid id, [FromBody] ReviewPutUpdateDto updateReviewDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await _context.Review.FirstOrDefaultAsync(r => r.Id == id);

            if (review is null) return NotFound();

            review.ReviewerName = updateReviewDto.ReviewerName;
            review.Comment = updateReviewDto.Comment;
            review.Rating = updateReviewDto.Rating;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Reviews/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(Guid id)
        {
            return _context.Review.Any(e => e.Id == id);
        }
    }
}
