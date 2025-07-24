using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Models.DTOs.ActorDtos;
using Movies.Core.Domain.Models.Entities;

namespace Movies.API.Controllers
{
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IServiceManager serviceManager;
        const int maxPageSize = 30;

        public ActorsController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }


        // GET: api/actors
        [HttpGet("api/actors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActor(int pageNumber = 1, int pageSize = 20)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var actorDtos = await serviceManager.ActorService.GetAllAsync();

            return Ok(actorDtos);
        }


        // GET: api/actors/5
        [HttpGet("api/actors/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Actor>> GetActor([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid actor ID" });

            var actorDto = await serviceManager.ActorService.GetAsync(id);

            if (actorDto == null)
            {
                return NotFound();
            }

            return Ok(actorDto);
        }


        // POST: api/actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/actors")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ActorDto>> PostActor([FromBody] ActorCreateDto createActorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actorDto = await serviceManager.ActorService.CreateAsync(createActorDto);

            return CreatedAtAction("GetActor", new { id = actorDto.Id }, actorDto);
        }


        // POST: api/movies/5/actors/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/movies/{movieId}/actors")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostActorToMovie([FromRoute] Guid movieId, [FromBody] MovieActorCreateDto maCreateDto)
        {
            if (movieId == Guid.Empty)
                return BadRequest(new { message = "Invalid movie ID" });

            try
            {
                var movieExists = await serviceManager.MovieService.AnyAsync(movieId);
                if (!movieExists)
                    return NotFound("Movie not found");

                var actorExists = await serviceManager.ActorService.AnyAsync(maCreateDto.ActorId);
                if (!actorExists)
                    return NotFound("Actor not found");

                var success = await serviceManager.ActorService.AddActorToMovieAsync(movieId, maCreateDto.ActorId, maCreateDto.Role);

                if (!success)
                    return BadRequest("Actor is already assigned to this movie");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"An internal server error occurred. \n {ex}" });
            }
        }


        // PUT: api/actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/actors/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutActor([FromRoute] Guid id, [FromBody] ActorPutUpdateDto updateActorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid actor ID" });

            try
            {
                var success = await serviceManager.ActorService.UpdateAsync(id, updateActorDto);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the actor" });
            }
        }

        // DELETE: api/actors/5
        [HttpDelete("api/actors/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteActor([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid actor ID" });

            try
            {
                var success = await serviceManager.ActorService.DeleteAsync(id);
                return success ? NoContent() : NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "The record was modified by another process" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while deleting the actor" });
            }
        }
    }
}
