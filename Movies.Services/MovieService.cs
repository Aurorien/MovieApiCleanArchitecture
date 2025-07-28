using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.MovieDtos;
using Movies.Core.Domain.Models.DTOs.ReviewDtos;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork uow;

        public MovieService(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        public async Task<bool> AnyAsync(Guid id) => await uow.Movies.AnyAsync(id);


        public async Task<(IEnumerable<MovieDto>, PaginationMetadata)> GetAllAsync(BaseRequestParams requestParams)
        {
            var (movies, paginationMetadata) = await uow.Movies.GetAllMoviesAsync(trackChanges: false, requestParams);

            return (movies.Select(MapToDto), paginationMetadata);
        }


        public async Task<MovieDto?> GetAsync(Guid id)
        {
            var movie = await uow.Movies.GetMovieAsync(id, trackChanges: false);

            return movie != null ? MapToDto(movie) : null;
        }


        public async Task<MovieDetailedDto?> GetDetailedAsync(Guid id)
        {
            var movie = await uow.Movies.GetMovieDetailedAsync(id, trackChanges: false);

            return movie != null ? MapToDetailedDto(movie) : null;
        }


        public async Task<MovieDto> CreateAsync(MovieCreateDto createDto)
        {
            var genreExists = await uow.Genres.AnyAsync(createDto.GenreId);
            if (!genreExists)
                throw new ArgumentException("Invalid genre ID");

            var movie = new Movie
            {
                Title = createDto.Title,
                Year = createDto.Year,
                GenreId = createDto.GenreId,
                DurationInMinutes = createDto.DurationInMinutes,
                MovieDetails = new MovieDetails
                {
                    Synopsis = createDto.Synopsis,
                    Language = createDto.Language,
                    Budget = createDto.Budget
                }

            };

            uow.Movies.Create(movie);
            await uow.CompleteAsync();

            return MapToDto(movie);
        }


        public async Task<bool> UpdateAsync(Guid id, MoviePutUpdateDto updateDto)
        {
            var movie = await uow.Movies.GetMovieAsync(id, trackChanges: true);
            if (movie == null)
            {
                return false;
            }

            movie.Title = updateDto.Title;
            movie.Year = updateDto.Year;
            movie.GenreId = updateDto.GenreId;
            movie.DurationInMinutes = updateDto.DurationInMinutes;
            movie.MovieDetails.Synopsis = updateDto.Synopsis;
            movie.MovieDetails.Language = updateDto.Language;
            movie.MovieDetails.Budget = updateDto.Budget;

            try
            {
                uow.Movies.Update(movie);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Movies.AnyAsync(id))
                    return false;

                throw;
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await uow.Movies.AnyAsync(id))
                return false;

            //Stub entity. No need to pass or get the whole entity for deletion, EF is only going to use the id of the entity to remove it anyway
            var movie = new Movie { Id = id };

            try
            {
                uow.Movies.Delete(movie);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Movies.AnyAsync(id))
                    return false;

                throw;
            }
        }


        private MovieDto MapToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre.Name,
                DurationInMinutes = movie.DurationInMinutes,
            };
        }


        private MovieDetailedDto MapToDetailedDto(Movie movie)
        {
            return new MovieDetailedDto
            {
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre.Name,
                DurationInMinutes = movie.DurationInMinutes,
                Synopsis = movie.MovieDetails.Synopsis,
                Language = movie.MovieDetails.Language,
                Budget = movie.MovieDetails.Budget,
                Actors = movie.MovieActors.Select(ma => new MovieActorDto
                {
                    Id = ma.Actor.Id,
                    FullName = ma.Actor.FullName,
                    BirthYear = ma.Actor.BirthYear,
                    Role = ma.Role,
                }),
                Reviews = movie.Reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    ReviewerName = r.ReviewerName,
                    Comment = r.Comment,
                    Rating = r.Rating
                })
            };
        }

    }
}
