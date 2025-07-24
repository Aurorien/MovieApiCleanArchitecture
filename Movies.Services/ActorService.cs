using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using System.Linq.Expressions;

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
        public async Task<IEnumerable<Actor>> GetAllAsync(bool trackChanges = false, params Expression<Func<Actor, object>>[] includes)
            => await uow.Actors.GetAllAsync(trackChanges, includes);

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

        public void Create(Actor actor) => uow.Actors.Create(actor);
        public void Update(Actor actor) => uow.Actors.Update(actor);
        public void Delete(Actor actor) => uow.Actors.Delete(actor);
    }
}
