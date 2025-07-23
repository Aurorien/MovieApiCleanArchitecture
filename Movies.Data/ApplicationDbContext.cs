using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Models.Entities;

namespace Movies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<Review> Review { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId });
        }
    }
}
