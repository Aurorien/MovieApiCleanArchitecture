using Microsoft.EntityFrameworkCore;
using Movies.Data;
using System.Diagnostics;

namespace Movies.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureDeletedAsync(); // Drops the database (if it exists)
            await context.Database.MigrateAsync(); // Creates the database + runs all migrations

            try
            {
                await VerifyCleanDatabase(context);
                await SeedData.InitAsync(context);
                Console.WriteLine("✅ Seed data completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Seed data failed: {ex.Message}");
                Debug.WriteLine(ex);
                throw;
            }

        }

        private static async Task VerifyCleanDatabase(ApplicationDbContext context)
        {
            var movieCount = await context.Movie.CountAsync();
            var genreCount = await context.Genre.CountAsync();
            var actorCount = await context.Actor.CountAsync();

            Console.WriteLine($"Database state: {movieCount} movies, {genreCount} genres, {actorCount} actors");

            if (movieCount > 0 || genreCount > 0 || actorCount > 0)
            {
                throw new InvalidOperationException(
                    "Database is not clean! " +
                    "Expected empty database for seeding. " +
                    "Make sure EnsureDeletedAsync() is called before MigrateAsync().");
            }
        }
    }
}
