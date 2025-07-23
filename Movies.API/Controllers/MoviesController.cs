using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.ActorDtos;
using Movies.Core.Domain.Models.DTOs.MovieDtos;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Domain.Models.Entities;

namespace Movies.API.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public MoviesController(IUnitOfWork uow)
        {
            _uow = uow;
        }


        // GET: api/movies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _uow.Movies.GetAllAsync(
                false,
                m => m.MovieDetails);

            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Genre = m.Genre,
                DurationInMinutes = m.DurationInMinutes,
                MovieDetailsLanguage = m.MovieDetails.Language
            })
            .ToList();

            return Ok(movieDtos);
        }


        // GET: api/movies/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> GetMovie([FromRoute] Guid id)
        {
            var movie = await _uow.Movies.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                MovieDetailsLanguage = movie.MovieDetails.Language
            };

            return Ok(movieDto);
        }


        // GET: api/movies/5/detailed
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDetailedDto>> GetMovieDetailed([FromRoute] Guid id)
        {
            var movie = await _uow.Movies.GetAsync(
                id,
                trackChanges: false,
                m => m.MovieDetails,
                m => m.MovieActors.Select(ma => ma.Actor.MovieActors.Select(am => am.Movie)),
                m => m.Reviews
                );
            if (movie == null)
                return NotFound();

            var movieDetailed = new MovieDetailedDto
            {
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                DurationInMinutes = movie.DurationInMinutes,
                Synopsis = movie.MovieDetails.Synopsis,
                Language = movie.MovieDetails.Language,
                Budget = movie.MovieDetails.Budget,
                Actors = movie.MovieActors.Select(ma => new ActorDto
                {
                    Id = ma.Actor.Id,
                    FullName = ma.Actor.FullName,
                    BirthYear = ma.Actor.BirthYear,
                    MovieTitles = ma.Actor.MovieActors.Select(am => new MovieTitlesDto
                    {
                        Id = am.Movie.Id,
                        Title = am.Movie.Title,
                        Role = am.Role
                    })
                }),
                Reviews = movie.Reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
            };

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

            _uow.Movies.Create(movie);
            await _uow.CompleteAsync();

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

            var movie = await _uow.Movies.GetAsync(
                id,
                trackChanges: false,
                m => m.MovieDetails,
                m => m.MovieActors.Select(ma => ma.Actor.MovieActors.Select(am => am.Movie)),
                m => m.Reviews
                );

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
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieExistsAsync(id))
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
            var movie = await _uow.Movies.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _uow.Movies.Delete(movie);
            await _uow.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> MovieExistsAsync(Guid id)
        {
            return await _uow.Movies.AnyAsync(id);
        }
    }
}
