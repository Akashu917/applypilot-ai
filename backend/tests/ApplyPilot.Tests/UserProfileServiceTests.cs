using ApplyPilot.Application.Common.Exceptions;
using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.Services;
using ApplyPilot.Application.Validators;
using ApplyPilot.Domain.Entities;
using ApplyPilot.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApplyPilot.Tests;

public class UserProfileServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly UserProfileService _service;

    public UserProfileServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        var validator = new UpdateProfileRequestValidator();
        _service = new UserProfileService(_db, validator);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task UpdateProfileAsync_ValidData_UpdatesUserProfile()
    {
        // Arrange
        var user = new User { Email = "developer@example.com" };
        var initialProfile = new UserProfile
        {
            UserId = user.Id,
            FirstName = "Akash",
            LastName = "Sharma",
            Country = "India",
            City = "Chennai"
        };
        user.Profile = initialProfile;
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var updateReq = new UpdateProfileRequest(
            "Akash",
            "Sharma",
            "Senior Full Stack Engineer (.NET + React)",
            "+919876543210",
            "India",
            "Chennai",
            "https://linkedin.com/in/akashsharma",
            "https://github.com/akashsharma",
            "https://akashsharma.dev",
            "INR"
        );

        // Act
        var result = await _service.UpdateProfileAsync(user.Id, updateReq);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Headline.Should().Be("Senior Full Stack Engineer (.NET + React)");
        result.Data.LinkedInUrl.Should().Be("https://linkedin.com/in/akashsharma");
        result.Data.PhoneNumber.Should().Be("+919876543210");

        var profileInDb = await _db.UserProfiles.FirstAsync(p => p.UserId == user.Id);
        profileInDb.Headline.Should().Be("Senior Full Stack Engineer (.NET + React)");
    }

    [Fact]
    public async Task UpdateProfileAsync_MissingRequiredFields_ThrowsValidationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateReq = new UpdateProfileRequest(
            "", // Empty first name
            "Sharma",
            null,
            null,
            "", // Empty country
            "Chennai",
            null,
            null,
            null,
            "INR"
        );

        // Act
        Func<Task> act = async () => await _service.UpdateProfileAsync(userId, updateReq);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}

