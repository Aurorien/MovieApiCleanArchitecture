using Microsoft.EntityFrameworkCore;
using MoviesApi.Models.Entities;

namespace MoviesApi.Data
{
    public class MoviesApiContext : DbContext
    {
        public MoviesApiContext(DbContextOptions<MoviesApiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<MoviesApi.Models.Entities.Actor> Actor { get; set; } = default!;
        public DbSet<MoviesApi.Models.Entities.Review> Review { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });
        }
    }
}
