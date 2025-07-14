using Microsoft.EntityFrameworkCore;
using Movies.API.Models.Entities;

namespace Movies.API.Data
{
    public class MoviesApiContext : DbContext
    {
        public MoviesApiContext(DbContextOptions<MoviesApiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Movies.API.Models.Entities.Actor> Actor { get; set; } = default!;
        public DbSet<Movies.API.Models.Entities.Review> Review { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });
        }
    }
}
