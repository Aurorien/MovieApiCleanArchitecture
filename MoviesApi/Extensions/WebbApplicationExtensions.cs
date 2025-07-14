using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using System.Diagnostics;

namespace MoviesApi.Extensions
{
    public static class WebbApplicationExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<MoviesApiContext>();

                await context.Database.EnsureDeletedAsync(); // Drops the database (if it exists)
                await context.Database.MigrateAsync(); // Creates the database + runs all migrations

                try
                {
                    await SeedData.InitAsync(context);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }
        }
    }
}
