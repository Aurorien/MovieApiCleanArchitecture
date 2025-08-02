using Moq;
using Movies.Core.Domain.Contracts;
using Movies.Core.Domain.Models.Entities;
using Xunit;

namespace Movies.Services.Tests
{
    public class ServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly MovieService _movieSut;

        public ServiceTests()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _movieSut = new MovieService(_mockUow.Object);
        }


        [Fact]
        public async Task IsGenreIdDocumentaryAsync_ShouldBeCaseInsensitiveAndReturnTrue_HappyPath()
        {
            //Arrange
            var documentaryGenre = new Genre { Id = Guid.NewGuid(), Name = "Documentary" };
            _mockUow.Setup(x => x.Genres.GetGenreAsync(It.IsAny<Guid>(), false, false)).ReturnsAsync(documentaryGenre);

            //Act
            var result = await _movieSut.IsGenreIdDocumentaryAsync(It.IsAny<Guid>());

            //Assert
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);
        }


        [Fact]
        public async Task IsMovieDocumentaryAsync_ShouldReturnTrue_HappyPath()
        {
            //Arrange
            const string documentaryGenre = "documentary";
            _mockUow.Setup(x => x.Movies.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true);
            _mockUow.Setup(x => x.Movies.IsMovieOfGenreAsync(It.IsAny<Guid>(), documentaryGenre)).ReturnsAsync(true);

            //Act
            var result = await _movieSut.IsMovieDocumentaryAsync(It.IsAny<Guid>());

            //Assert
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);
        }


        [Fact]
        public async Task IsDocumentaryActorLimitReachedAsync_ShouldReturnTrue_SadPath()
        {
            //Arrange
            _mockUow.Setup(x => x.Movies.AnyAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mocking underlying repo call in IsMovieDocumentaryAsync
            _mockUow.Setup(x => x.Movies.IsMovieOfGenreAsync(It.IsAny<Guid>(), "documentary")).ReturnsAsync(true);
            _mockUow.Setup(x => x.Actors.NumberOfActorsInMovieAsync(It.IsAny<Guid>())).ReturnsAsync(10);

            // Act
            var result = await _movieSut.IsDocumentaryActorLimitReachedAsync(It.IsAny<Guid>());

            // Assert
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);
        }


        [Fact]
        public async Task IsDocumentaryBudgetLimitReachedAsync_ShouldReturnTrue_SadPath()
        {
            //Arrange
            const int newBudget = 1500000;
            var documentaryGenre = new Genre { Id = Guid.NewGuid(), Name = "Documentary" };
            _mockUow.Setup(x => x.Genres.GetGenreAsync(It.IsAny<Guid>(), false, false)).ReturnsAsync(documentaryGenre); // Mocking underlying repo call in IsGenreIdDocumentaryAsync

            // Act
            var result = await _movieSut.IsDocumentaryBudgetLimitReachedAsync(It.IsAny<Guid>(), newBudget);

            // Assert
            var resultType = Assert.IsType<bool>(result);
            Assert.True(resultType);
        }

    }
}