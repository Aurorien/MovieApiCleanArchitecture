using Microsoft.EntityFrameworkCore;
using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.DTOs.ActorDtos;
using Movies.Core.Domain.Models.DTOs.MovieDtos;
using Movies.Core.Domain.Models.Entities;
using Movies.Core.Requests;

namespace Movies.Services
{
    public class ActorService : IActorService
    {
        private readonly IUnitOfWork uow;

        public ActorService(IUnitOfWork uow)
        {
            this.uow = uow;
        }


        public async Task<bool> AnyAsync(Guid id) => await uow.Actors.AnyAsync(id);


        public async Task<(IEnumerable<ActorDto>, PaginationMetadata)> GetAllAsync(BaseRequestParams requestParams)
        {
            var (actors, paginationMetadata) = await uow.Actors.GetAllActorsAsync(trackChanges: false, requestParams);

            return (actors.Select(MapToDto), paginationMetadata);
        }


        public async Task<ActorDto?> GetAsync(Guid id)
        {
            var actor = await uow.Actors.GetActorAsync(id, trackChanges: false);

            return actor != null ? MapToDto(actor) : null;
        }


        public async Task<ActorDto> CreateAsync(ActorCreateDto createDto)
        {
            var actor = new Actor
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                BirthYear = createDto.BirthYear,
            };

            uow.Actors.Create(actor);
            await uow.CompleteAsync();

            return MapToDto(actor);
        }


        public async Task<bool> IsActorInMovieAsync(Guid movieId, Guid actorId)
        {
            return await uow.Actors.IsActorInMovieAsync(movieId, actorId);
        }


        public async Task<bool> AddActorToMovieAsync(Guid movieId, Guid actorId, string role)
        {
            if (await uow.Actors.IsActorInMovieAsync(movieId, actorId))
            {
                return false;
            }

            var movieActor = new MovieActor
            {
                MovieId = movieId,
                ActorId = actorId,
                Role = role
            };

            try
            {
                uow.Actors.AddActorToMovie(movieActor);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint") == true)
                {
                    throw new InvalidOperationException("Invalid movie or actor reference", ex);
                }

                throw new InvalidOperationException("Unable to add actor to movie due to a database error", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while adding actor to movie", ex);
            }
        }


        public async Task<bool> UpdateAsync(Guid id, ActorPutUpdateDto updateDto)
        {
            var actor = await uow.Actors.GetActorAsync(id, trackChanges: true);
            if (actor == null)
            {
                return false;
            }

            actor.FirstName = updateDto.FirstName;
            actor.LastName = updateDto.LastName;
            actor.BirthYear = updateDto.BirthYear;

            try
            {
                uow.Actors.Update(actor);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Actors.AnyAsync(id))
                    return false;

                throw;
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            if (!await uow.Actors.AnyAsync(id))
                return false;

            //Stub entity. No need to pass or get the whole entity for deletion, EF is only going to use the id of the entity to remove it anyway
            var actor = new Actor { Id = id };

            try
            {
                uow.Actors.Delete(actor);
                await uow.CompleteAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await uow.Actors.AnyAsync(id))
                    return false;

                throw;
            }
        }


        private ActorDto MapToDto(Actor actor)
        {
            return new ActorDto
            {
                Id = actor.Id,
                FullName = actor.FullName,
                BirthYear = actor.BirthYear,
                MovieTitles = actor.MovieActors?.Select(ma => new MovieTitlesDto
                {
                    Id = ma.Movie.Id,
                    Title = ma.Movie.Title,
                    Role = ma.Role
                }) ?? Enumerable.Empty<MovieTitlesDto>()
            };
        }
    }
}
