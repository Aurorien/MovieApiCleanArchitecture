namespace Movies.Core.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IActorRepository Actors { get; }
        IReviewRepository Reviews { get; }
        Task CompleteAsync();
    }
}
