using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Requests;
using System.Text.Json;

namespace Movies.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IServiceManager serviceManager;
        const int maxPageSize = 100;

        public ReviewsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }


        // GET: api/Reviews
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews([FromQuery] BaseRequestParams requestParams)
        {
            var (reviewDtos, paginationMetadata) = await serviceManager.ReviewService.GetAllAsync(requestParams);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(reviewDtos);
        }


        // GET: api/Reviews/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDto>> GetReview([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid review ID" });

            var reviewDto = await serviceManager.ReviewService.GetAsync(id);

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
        public async Task<ActionResult<ReviewDto>> PostReview([FromBody] ReviewCreateDto createDto)
        {
            var isMaxReviews = await serviceManager.ReviewService.IsMaxReviews(createDto.MovieId);
            if (isMaxReviews)
                return BadRequest(new { message = "Not possible to add more reviews to movie. Max limit of reviews for movie was reached." });

            var reviewDto = await serviceManager.ReviewService.CreateAsync(createDto);

            return CreatedAtAction("GetReview", new { id = reviewDto.Id }, reviewDto);
        }


        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutReview([FromRoute] Guid id, [FromBody] GenrePutUpdateDto updateDto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid review ID" });

            try
            {
                var success = await serviceManager.ReviewService.UpdateAsync(id, updateDto);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the review" });
            }
        }


        // DELETE: api/Reviews/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid review ID" });

            try
            {
                var success = await serviceManager.ReviewService.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the review" });
            }
        }
    }
}
