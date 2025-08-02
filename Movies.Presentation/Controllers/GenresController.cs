using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Services.Contracts;
using Movies.Core.Domain.Models.DTOs.GenreDtos;
using Movies.Core.Requests;
using System.Text.Json;

namespace Movies.Presentation.Controllers
{

    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IServiceManager serviceManager;
        const int maxPageSize = 100;

        public GenresController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }


        // GET: api/Genres
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres([FromQuery] BaseRequestParams requestParams)
        {
            var (genreDtos, paginationMetadata) = await serviceManager.GenreService.GetAllAsync(requestParams);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(genreDtos);
        }


        // GET: api/Genres/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreDto>> GetGenre([FromRoute] Guid id, [FromQuery] bool includeMovies = false)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid empty genre ID" });

            var genreDto = await serviceManager.GenreService.GetAsync(id, includeMovies);

            if (genreDto == null)
            {
                return NotFound();
            }

            return Ok(genreDto);
        }


        // POST: api/genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenreDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenreDto>> PostGenre([FromBody] GenreCreateDto createDto)
        {
            try
            {
                var genreDto = await serviceManager.GenreService.CreateAsync(createDto);
                return CreatedAtAction("GetGenre", new { id = genreDto.Id }, genreDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }


        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutGenre([FromRoute] Guid id, [FromBody] GenreUpdateDto updateDto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid empty genre ID" });

            try
            {
                var success = await serviceManager.GenreService.UpdateAsync(id, updateDto);
                return success ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the genre" });
            }
        }


        // DELETE: api/Genres/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGenre([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid empty genre ID" });

            try
            {
                var success = await serviceManager.GenreService.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the genre" });
            }
        }
    }
}



