using Bogus;
using Microsoft.EntityFrameworkCore;
using Movies.Core.Domain.Models.Entities;
using System.Diagnostics;
using System.Globalization;

namespace Movies.Data
{
    public class SeedData
    {
        private static Faker faker = new Faker();

        private static readonly string[] _genres =
        {
            "Action", "Adventure", "Comedy", "Drama", "Fantasy",
            "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller",
            "Western", "Crime", "Documentary", "Animation", "Musical"
        };

        private static int _maxAvailableGenres => _genres.Length;


        public static async Task InitAsync(ApplicationDbContext context)
        {

            if (await context.Movie.AnyAsync()) return;

            var actors = GenerateActors(3);
            await context.AddRangeAsync(actors);

            var genres = GenerateUniqueGenres(10);
            await context.AddRangeAsync(genres);

            await context.SaveChangesAsync();

            var movies = GenerateMovies(genres, 20);
            await context.AddRangeAsync(movies);

            await context.SaveChangesAsync();

            var movieDetails = GenerateMovieDetails(movies);
            await context.AddRangeAsync(movieDetails);

            await context.SaveChangesAsync();

            var movieActors = GenerateMovieActors(movies, actors);
            await context.AddRangeAsync(movieActors);

            var reviews = GenerateReviews(movies, 50);
            await context.AddRangeAsync(reviews);

            await context.SaveChangesAsync();
        }


        private static List<MovieActor> GenerateMovieActors(List<Movie> movies, List<Actor> actors)
        {
            var movieActors = new List<MovieActor>();

            foreach (var movie in movies)
            {
                // Randomly assigning 1-3 actors to each movie
                var numberOfActors = faker.Random.Int(1, Math.Min(3, actors.Count()));
                var selectedActors = faker.PickRandom(actors, numberOfActors);

                foreach (var actor in selectedActors)
                {
                    var role = faker.Name.JobTitle();

                    var movieActor = new MovieActor
                    {
                        Actor = actor,
                        Movie = movie,
                        Role = role
                    };

                    movieActors.Add(movieActor);
                }
            }

            return movieActors;
        }


        private static List<Movie> GenerateMovies(List<Genre> genres, int numberOfMovies)
        {
            var movies = new List<Movie>();

            for (int i = 0; i < numberOfMovies; i++)
            {
                //var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Commerce.ProductName());
                var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.CompanyName());
                var year = faker.Random.Int(1878, 2100);
                var genre = faker.PickRandom(genres);
                var durationInMinutes = faker.Random.Int(1, 55000);

                var movie = new Movie
                {
                    Title = title,
                    Year = year,
                    DurationInMinutes = durationInMinutes,
                    GenreId = genre.Id
                };

                movies.Add(movie);
            }

            return movies;
        }


        private static List<MovieDetails> GenerateMovieDetails(List<Movie> movies)
        {
            var movieDetails = new List<MovieDetails>();

            foreach (var movie in movies)
            {
                var movieDetail = new MovieDetails
                {
                    Synopsis = faker.Lorem.Paragraph(5),
                    Language = GetRandomLanguage(),
                    Budget = faker.Random.Int(1, int.MaxValue),
                    MovieId = movie.Id
                };

                movieDetails.Add(movieDetail);
            }

            return movieDetails;
        }


        private static List<Actor> GenerateActors(int numberOfActors)
        {
            var actors = new List<Actor>();

            for (int i = 0; i < numberOfActors; i++)
            {
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();
                var birthYear = faker.Random.Int(1850, DateTime.Now.Year);

                var actor = new Actor
                {
                    FirstName = fName,
                    LastName = lName,
                    BirthYear = birthYear
                };

                actors.Add(actor);
            }

            return actors;
        }


        private static List<Genre> GenerateUniqueGenres(int count)
        {
            if (count > _maxAvailableGenres)
            {
                Debug.WriteLine($"Requested {count} genres reduced to {_maxAvailableGenres}");
                count = _maxAvailableGenres;
            }

            var faker = new Faker();
            return faker.PickRandom(_genres, count)
                       .Select(name => new Genre { Name = name })
            .ToList();
        }


        private static List<Review> GenerateReviews(List<Movie> movies, int totalReviews)
        {
            var reviews = new List<Review>();

            for (int i = 0; i < totalReviews; i++)
            {
                var movie = faker.PickRandom(movies);

                var review = new Review
                {
                    ReviewerName = faker.Name.FullName(),
                    Comment = faker.Lorem.Sentence(faker.Random.Int(5, 20)),
                    Rating = faker.Random.Int(1, 5),
                    MovieId = movie.Id
                };

                reviews.Add(review);
            }

            return reviews;
        }


        private static string GetRandomLanguage()
        {
            var cultures = new[]
            {
                "en-US", // English
                "es-ES", // Spanish
                "fr-FR", // French
                "de-DE", // German
                "it-IT", // Italian
                "ja-JP", // Japanese
                "pt-BR", // Portuguese
                "ru-RU", // Russian
                "zh-CN", // Chinese
                "ko-KR"  // Korean
            };

            return faker.PickRandom(cultures);
        }
    }
}


