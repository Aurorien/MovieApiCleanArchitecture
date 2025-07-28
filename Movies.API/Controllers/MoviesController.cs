using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Models.DTOs.MovieDtos;
using Movies.Core.Requests;
using System.Text.Json;

namespace Movies.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IServiceManager serviceManager;
        const int maxPageSize = 100;

        public MoviesController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }


        // GET: api/movies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies([FromQuery] BaseRequestParams requestParams)
        {
            var (movieDtos, paginationMetadata) = await serviceManager.MovieService.GetAllAsync(requestParams);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(movieDtos);
        }


        // GET: api/movies/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> GetMovie([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid movie ID" });

            var movieDto = await serviceManager.MovieService.GetAsync(id);

            if (movieDto == null)
            {
                return NotFound();
            }

            return Ok(movieDto);
        }


        // GET: api/movies/5/detailed
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDetailedDto>> GetMovieDetailed([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid movie ID" });

            var movieDetailedDto = await serviceManager.MovieService.GetDetailedAsync(id, trackChanges: false);
            if (movieDetailedDto == null)
                return NotFound();

            return Ok(movieDetailedDto);
        }


        // POST: api/movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieDto = await serviceManager.MovieService.CreateAsync(createDto);

            return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
        }


        // PUT: api/movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutMovie([FromRoute] Guid id, [FromBody] MoviePutUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid movie ID" });

            try
            {
                var success = await serviceManager.MovieService.UpdateAsync(id, updateDto);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the movie" });
            }
        }


        // DELETE: api/movies/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid movie ID" });

            try
            {
                var success = await serviceManager.MovieService.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the movie" });
            }
        }
    }
}
