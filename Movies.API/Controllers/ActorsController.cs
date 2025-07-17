using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Core.Models.DTOs.ActorDtos;
using Movies.Core.Models.DTOs.MovieDtos;
using Movies.Core.Models.Entities;
using Movies.Data;

namespace Movies.API.Controllers
{
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/actors
        [HttpGet("api/actors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActor()
        {
            var actorDtos = await _context.Actor
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthYear = a.BirthYear,
                    MovieTitles = a.MovieActors.Select(ma => new MovieTitlesDto
                    {
                        Id = ma.Movie.Id,
                        Title = ma.Movie.Title,
                        Role = ma.Role
                    }),
                })
                .ToListAsync();


            return Ok(actorDtos);
        }


        // GET: api/actors/5
        [HttpGet("api/actors{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Actor>> GetActor([FromRoute] Guid id)
        {
            var actorDto = await _context.Actor
                .Where(a => a.Id == id)
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthYear = a.BirthYear,
                    MovieTitles = a.MovieActors.Select(ma => new MovieTitlesDto
                    {
                        Id = ma.Movie.Id,
                        Title = ma.Movie.Title,
                        Role = ma.Role
                    }),
                })
                .FirstOrDefaultAsync();

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

            var actor = new Actor
            {
                FirstName = createActorDto.FirstName,
                LastName = createActorDto.LastName,
                BirthYear = createActorDto.BirthYear,
            };

            _context.Actor.Add(actor);
            await _context.SaveChangesAsync();

            var actorDto = new ActorDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                BirthYear = actor.BirthYear,
                MovieTitles = actor.MovieActors.Select(ma => new MovieTitlesDto
                {
                    Id = ma.Movie.Id,
                    Title = ma.Movie.Title,
                    Role = ma.Role
                }),
            };

            return CreatedAtAction("GetActor", new { id = actorDto.Id }, actorDto);
        }


        // POST: api/movies/5/actors/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/movies/{movieId}/actors")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostActorToMovie([FromRoute] Guid movieId, [FromBody] MovieActorCreateDto maCreateDto)
        {

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie == null) return NotFound("Movie not found");

            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.Id == maCreateDto.ActorId);
            if (actor == null) return NotFound("Actor not found");

            var isActorAlreadyInMovie = await _context.Set<MovieActor>()
                                                      .AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == maCreateDto.ActorId);

            if (isActorAlreadyInMovie)
                return BadRequest("Actor is already assigned to this movie");

            var movieActor = new MovieActor
            {
                MovieId = movieId,
                ActorId = maCreateDto.ActorId,
                Role = maCreateDto.Role
            };

            _context.Set<MovieActor>().Add(movieActor);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // PUT: api/actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/actors{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutActor([FromRoute] Guid id, [FromBody] ActorPutUpdateDto updateActorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actor = await _context.Actor.FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null) return NotFound();

            actor.FirstName = updateActorDto.FirstName;
            actor.LastName = updateActorDto.LastName;
            actor.BirthYear = updateActorDto.BirthYear;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
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


        private bool ActorExists(Guid id)
        {
            return _context.Actor.Any(e => e.Id == id);
        }
    }
}
