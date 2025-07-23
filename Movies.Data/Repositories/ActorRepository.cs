using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Data.Repositories
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context) { }
    }
}
