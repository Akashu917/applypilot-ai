using ApplyPilot.Domain.Entities;
using ApplyPilot.Domain.Enums;
using ApplyPilot.Infrastructure.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace ApplyPilot.Tests;

public class JwtServiceTests
{
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
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
    }

    [Fact]
    public void GenerateAccessToken_ReturnsValidJwtAndExtractsUserId()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "tester@applypilot.ai",
            Role = UserRole.Candidate,
            Profile = new UserProfile
            {
                FirstName = "Test",
                LastName = "User"
            }
        };

        // Act
        var token = _jwtService.GenerateAccessToken(user);
        var extractedUserId = _jwtService.ValidateAccessToken(token);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
        extractedUserId.Should().Be(user.Id);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsRandomUniqueTokens()
    {
        // Act
        var token1 = _jwtService.GenerateRefreshToken();
        var token2 = _jwtService.GenerateRefreshToken();

        // Assert
        token1.Should().NotBeNullOrWhiteSpace();
        token2.Should().NotBeNullOrWhiteSpace();
        token1.Should().NotBe(token2);
    }

    [Fact]
    public void HashToken_ProducesDeterministicSha256()
    {
        // Arrange
        var rawToken = "sample-token-string-xyz";

        // Act
        var hash1 = _jwtService.HashToken(rawToken);
        var hash2 = _jwtService.HashToken(rawToken);

        // Assert
        hash1.Should().Be(hash2);
    }
}

