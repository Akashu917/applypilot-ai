using ApplyPilot.Application.Common.Exceptions;
using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.Services;
using ApplyPilot.Application.Validators;
using ApplyPilot.Domain.Entities;
using ApplyPilot.Domain.Enums;
using ApplyPilot.Infrastructure.Persistence;
using ApplyPilot.Infrastructure.Security;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApplyPilot.Tests;

public class AuthServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly JwtService _jwtService;
    private readonly BcryptPasswordHasher _hasher;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Secret", "ApplyPilotAI_SuperSecretKey_ProductionGrade_2026_Minimum256BitsRequired!" },
                { "Jwt:Issuer", "ApplyPilot.Api" },
                { "Jwt:Audience", "ApplyPilot.Client" },
                { "Jwt:AccessTokenLifetimeMinutes", "60" }
            })
            .Build();

        _jwtService = new JwtService(config);
        _hasher = new BcryptPasswordHasher();

        var registerValidator = new RegisterRequestValidator();
        var loginValidator = new LoginRequestValidator();

        _authService = new AuthService(_db, _hasher, _jwtService, registerValidator, loginValidator);
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task RegisterAsync_ValidRequest_CreatesUserAndReturnsTokens()
    {
        // Arrange
        var request = new RegisterRequest(
            "rahul.sharma@example.com",
            "Password123!",
            "Rahul",
            "Sharma",
            "India",
            "Bengaluru"
        );

        // Act
        var response = await _authService.RegisterAsync(request);

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        response.Data.User.Email.Should().Be("rahul.sharma@example.com");
        response.Data.User.Role.Should().Be(UserRole.Candidate.ToString());
        response.Data.User.Profile.Should().NotBeNull();
        response.Data.User.Profile!.FirstName.Should().Be("Rahul");
        response.Data.User.Profile.PreferredCurrency.Should().Be("INR");

        var userInDb = await _db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Email == "rahul.sharma@example.com");
        userInDb.Should().NotBeNull();
        userInDb!.Profile.Should().NotBeNull();
        _hasher.Verify("Password123!", userInDb.PasswordHash!).Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ThrowsConflictException()
    {
        // Arrange
        var request = new RegisterRequest("priya@example.com", "Password123!", "Priya", "Nair", "India", "Kochi");
        await _authService.RegisterAsync(request);

        // Act
        Func<Task> act = async () => await _authService.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<AppException>()
            .Where(e => e.StatusCode == 409);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var registerReq = new RegisterRequest("amit.kumar@example.com", "SecurePass123!", "Amit", "Kumar", "India", "Delhi");
        await _authService.RegisterAsync(registerReq);

        var loginReq = new LoginRequest("amit.kumar@example.com", "SecurePass123!");

        // Act
        var response = await _authService.LoginAsync(loginReq);

        // Assert
        response.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.User.Email.Should().Be("amit.kumar@example.com");
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedException()
    {
        // Arrange
        var registerReq = new RegisterRequest("deepak@example.com", "SecurePass123!", "Deepak", "Verma", "India", "Pune");
        await _authService.RegisterAsync(registerReq);

        var loginReq = new LoginRequest("deepak@example.com", "WrongPassword999!");

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(loginReq);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidToken_RotatesTokenCorrectly()
    {
        // Arrange
        var registerReq = new RegisterRequest("ananya@example.com", "StrongPassword123!", "Ananya", "Sen", "India", "Kolkata");
        var registerRes = await _authService.RegisterAsync(registerReq);
        var initialRefreshToken = registerRes.Data!.RefreshToken;

        // Act
        var refreshRes = await _authService.RefreshTokenAsync(initialRefreshToken);

        // Assert
        refreshRes.Success.Should().BeTrue();
        refreshRes.Data!.RefreshToken.Should().NotBe(initialRefreshToken);

        // Old token should now be revoked
        var oldHash = _jwtService.HashToken(initialRefreshToken);
        var oldRecord = await _db.RefreshTokens.FirstAsync(rt => rt.TokenHash == oldHash);
        oldRecord.IsRevoked.Should().BeTrue();
        oldRecord.ReplacedByTokenHash.Should().NotBeNull();

        // Attempting to refresh with old token should fail
        Func<Task> reuseAct = async () => await _authService.RefreshTokenAsync(initialRefreshToken);
        await reuseAct.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task RevokeTokenAsync_RevokesActiveToken()
    {
        // Arrange
        var registerReq = new RegisterRequest("sanjay@example.com", "SecurePass123!", "Sanjay", "Rao", "India", "Hyderabad");
        var registerRes = await _authService.RegisterAsync(registerReq);
        var refreshToken = registerRes.Data!.RefreshToken;

        // Act
        var revokeRes = await _authService.RevokeTokenAsync(refreshToken);

        // Assert
        revokeRes.Success.Should().BeTrue();
        var tokenHash = _jwtService.HashToken(refreshToken);
        var record = await _db.RefreshTokens.FirstAsync(rt => rt.TokenHash == tokenHash);
        record.IsRevoked.Should().BeTrue();
    }
}
