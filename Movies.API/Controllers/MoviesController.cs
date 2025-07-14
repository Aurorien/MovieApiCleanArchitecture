using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.API.Data;
using Movies.API.Models.DTOs.ActorDtos;
using Movies.API.Models.DTOs.MovieDtos;
using Movies.API.Models.DTOs.ReviewDtos;
using Movies.API.Models.Entities;

namespace Movies.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesApiContext _context;

        public MoviesController(MoviesApiContext context)
        {
            _context = context;
        }


        // GET: api/movies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movieDtos = await _context.Movie
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    MovieDetailsLanguage = m.MovieDetails.Language
                })
                .ToListAsync();

            return Ok(movieDtos);
        }


        // GET: api/movies/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> GetMovie([FromRoute] Guid id)
        {
            var movieDto = await _context.Movie
                .Where(m => m.Id == id)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    MovieDetailsLanguage = m.MovieDetails.Language
                })
                .FirstOrDefaultAsync();

            if (movieDto == null)
            {
                return NotFound();
            }

            return Ok(movieDto);
        }


        // GET: api/movies/5/detailed
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDetailedDto>> GetMovieDetailed([FromRoute] Guid id)
        {
            var movieDetailed = await _context.Movie
                .Where(m => m.Id == id)
                .Include(m => m.MovieDetails)
                .Select(m => new MovieDetailedDto
                {
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre,
                    DurationInMinutes = m.DurationInMinutes,
                    Synopsis = m.MovieDetails.Synopsis,
                    Language = m.MovieDetails.Language,
                    Budget = m.MovieDetails.Budget,
                    Actors = m.MovieActors.Select(ma => new ActorDto
                    {
                        Id = ma.Actor.Id,
                        FullName = ma.Actor.FullName,
                        BirthYear = ma.Actor.BirthYear,
                        MovieTitles = ma.Actor.MovieActors.Select(ma => new MovieTitlesDto
                        {
                            Id = ma.Movie.Id,
                            Title = ma.Movie.Title,
                            Role = ma.Role
                        }),
                    }),
                    Reviews = m.Reviews.Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        ReviewerName = r.ReviewerName,
                        Comment = r.Comment,
                        Rating = r.Rating
                    })
                })
                .FirstOrDefaultAsync();


            if (movieDetailed is null) return NotFound();

            return Ok(movieDetailed);
        }


        // POST: api/movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieCreateDto createMovieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Title = createMovieDto.Title,
                Year = createMovieDto.Year,
                Genre = createMovieDto.Genre,
                DurationInMinutes = createMovieDto.DurationInMinutes,
                MovieDetails = new MovieDetails
                {
                    Synopsis = createMovieDto.Synopsis,
                    Language = createMovieDto.Language,
                    Budget = createMovieDto.Budget
                }

            };

            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                MovieDetailsLanguage = movie.MovieDetails.Language
            };

            return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
        }


        // PUT: api/movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMovie([FromRoute] Guid id, [FromBody] MoviePutUpdateDto updateMovieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _context.Movie
                   .Include(m => m.MovieDetails)
                   .Include(m => m.MovieActors)
                        .ThenInclude(ma => ma.Actor)
                   .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null) return NotFound();

            movie.Title = updateMovieDto.Title;
            movie.Year = updateMovieDto.Year;
            movie.Genre = updateMovieDto.Genre;
            movie.DurationInMinutes = updateMovieDto.DurationInMinutes;
            movie.MovieDetails.Synopsis = updateMovieDto.Synopsis;
            movie.MovieDetails.Language = updateMovieDto.Language;
            movie.MovieDetails.Budget = updateMovieDto.Budget;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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


        // DELETE: api/movies/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMovie([FromRoute] Guid id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(Guid id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
