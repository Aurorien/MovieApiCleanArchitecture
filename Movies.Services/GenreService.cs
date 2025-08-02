using Microsoft.EntityFrameworkCore;
using Movies.Services.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.GenreDtos;
using Movies.Core.Domain.Models.DTOs.MovieDtos;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork uow;

        public GenreService(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        public async Task<bool> AnyAsync(Guid id) => await uow.Genres.AnyAsync(id);


        public async Task<(IEnumerable<GenreDto>, PaginationMetadata)> GetAllAsync(BaseRequestParams requestParams)
        {
            var (genres, paginationMetadata) = await uow.Genres.GetAllGenresAsync(trackChanges: false, requestParams);

            return (genres.Select(MapToDto), paginationMetadata);
        }


        public async Task<GenreDto?> GetAsync(Guid id, bool includeMovies)
        {
            var genre = await uow.Genres.GetGenreAsync(id, trackChanges: false, includeMovies);

            return genre != null ?
                             (includeMovies ? MapToGenreMoviesDto(genre) : MapToDto(genre))
                            : null;
        }


        public async Task<GenreDto> CreateAsync(GenreCreateDto createDto)
        {
            var nameExists = await uow.Genres.ExistsByNameAsync(createDto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException($"A genre with the name '{createDto.Name}' already exists.");
            }

            var genre = new Genre
            {
                Name = createDto.Name,
            };

            uow.Genres.Create(genre);
            await uow.CompleteAsync();

            return MapToDto(genre);
        }


        public async Task<bool> UpdateAsync(Guid id, GenreUpdateDto updateDto)
        {
            var genre = await uow.Genres.GetGenreAsync(id, trackChanges: true, includeMovies: false);
            if (genre == null)
            {
                return false;
            }

            var nameExists = await uow.Genres.ExistsByNameAsync(updateDto.Name, excludeId: id);
            if (nameExists)
            {
                throw new InvalidOperationException($"A genre with the name '{updateDto.Name}' already exists.");
            }

            genre.Name = updateDto.Name;

            try
            {
                uow.Genres.Update(genre);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Genres.AnyAsync(id))
                    return false;

                throw;
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await uow.Genres.AnyAsync(id))
                return false;

            //Stub entity. No need to pass or get the whole entity for deletion, EF is only going to use the id of the entity to remove it anyway
            var genre = new Genre { Id = id };

            try
            {
                uow.Genres.Delete(genre);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Genres.AnyAsync(id))
                    return false;

                throw;
            }
        }


        private GenreDto MapToDto(Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
            };
        }


        private GenreMoviesDto MapToGenreMoviesDto(Genre genre)
        {
            return new GenreMoviesDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Movies = genre.Movies.Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Genre = m.Genre.Name,
                    DurationInMinutes = m.DurationInMinutes,
                })
            };
        }
    }
}
