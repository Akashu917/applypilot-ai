using ApplyPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplyPilot.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashToken(string token);
    Guid? ValidateAccessToken(string token);
}

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string firstName);
    Task SendVerificationEmailAsync(string toEmail, string token);
    Task SendPasswordResetEmailAsync(string toEmail, string token);
}

