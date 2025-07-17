using Movies.Core.DomainContracts;
using Movies.Core.Models.Entities;

namespace Movies.Data.Repositories
{
    public class ActorRepository : RepositoryBase<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context) { }
    }
}
