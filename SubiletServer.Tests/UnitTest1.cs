using Xunit;
using FluentAssertions;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Tests
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "john@example.com";
            var username = "johndoe";
            var password = "password123";

            // Act
            var user = new User(firstName, lastName, email, username, password);

            // Assert
            user.Should().NotBeNull();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Email.Should().Be(email);
            user.Username.Should().Be(username);
            user.IsActive.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
        {
            // Arrange
            var user = new User("John", "Doe", "john@example.com", "johndoe", "password123");

            // Act
            var result = user.VerifyPasswordHash("password123");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
        {
            // Arrange
            var user = new User("John", "Doe", "john@example.com", "johndoe", "password123");

            // Act
            var result = user.VerifyPasswordHash("wrongpassword");

            // Assert
            result.Should().BeFalse();
        }
    }

    public class MusicEventTests
    {
        [Fact]
        public void CreateMusicEvent_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var artistName = "Test Artist";
            var description = "Test Description";
            var date = DateTime.Now.AddDays(30);
            var location = "Test Location";
            var price = 100.0m;
            var capacity = 1000;
            var imageUrl = "/images/test.jpg";
            var genre = MusicGenre.Pop;

            // Act
            var musicEvent = new MusicEvent
            {
                ArtistName = artistName,
                Description = description,
                Date = date,
                Location = location,
                Price = price,
                Capacity = capacity,
                ImageUrl = imageUrl,
                Genre = genre
            };

            // Assert
            musicEvent.Should().NotBeNull();
            musicEvent.ArtistName.Should().Be(artistName);
            musicEvent.Description.Should().Be(description);
            musicEvent.Date.Should().Be(date);
            musicEvent.Location.Should().Be(location);
            musicEvent.Price.Should().Be(price);
            musicEvent.Capacity.Should().Be(capacity);
            musicEvent.ImageUrl.Should().Be(imageUrl);
            musicEvent.Genre.Should().Be(genre);
            musicEvent.Status.Should().Be(EventStatus.Active);
        }
    }

    public class SportEventTests
    {
        [Fact]
        public void CreateSportEvent_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var title = "Test Match";
            var description = "Test Description";
            var date = DateTime.Now.AddDays(30);
            var location = "Test Stadium";
            var price = 75.0m;
            var capacity = 50000;
            var imageUrl = "/images/test.jpg";
            var genre = SportGenre.Football;

            // Act
            var sportEvent = new SportEvent
            {
                Title = title,
                Description = description,
                Date = date,
                Location = location,
                Price = price,
                Capacity = capacity,
                ImageUrl = imageUrl,
                Genre = genre
            };

            // Assert
            sportEvent.Should().NotBeNull();
            sportEvent.Title.Should().Be(title);
            sportEvent.Description.Should().Be(description);
            sportEvent.Date.Should().Be(date);
            sportEvent.Location.Should().Be(location);
            sportEvent.Price.Should().Be(price);
            sportEvent.Capacity.Should().Be(capacity);
            sportEvent.ImageUrl.Should().Be(imageUrl);
            sportEvent.Genre.Should().Be(genre);
            sportEvent.Status.Should().Be(EventStatus.Active);
        }
    }

    public class StageEventTests
    {
        [Fact]
        public void CreateStageEvent_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var title = "Test Play";
            var description = "Test Description";
            var date = DateTime.Now.AddDays(30);
            var location = "Test Theatre";
            var price = 200.0m;
            var capacity = 500;
            var imageUrl = "/images/test.jpg";
            var genre = StageGenre.Theatre;

            // Act
            var stageEvent = new StageEvent
            {
                Title = title,
                Description = description,
                Date = date,
                Location = location,
                Price = price,
                Capacity = capacity,
                ImageUrl = imageUrl,
                Genre = genre
            };

            // Assert
            stageEvent.Should().NotBeNull();
            stageEvent.Title.Should().Be(title);
            stageEvent.Description.Should().Be(description);
            stageEvent.Date.Should().Be(date);
            stageEvent.Location.Should().Be(location);
            stageEvent.Price.Should().Be(price);
            stageEvent.Capacity.Should().Be(capacity);
            stageEvent.ImageUrl.Should().Be(imageUrl);
            stageEvent.Genre.Should().Be(genre);
            stageEvent.Status.Should().Be(EventStatus.Active);
        }
    }
}
