using ApplyPilot.Application.Common.Exceptions;
using ApplyPilot.Application.Common.Interfaces;
using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.DTOs.Common;
using ApplyPilot.Domain.Entities;
using ApplyPilot.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ApplyPilot.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthService(
        IAppDbContext db,
        IPasswordHasher hasher,
        IJwtService jwt,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var validationResult = await _registerValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var failures = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Common.Exceptions.ValidationException(failures);
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await _db.Users.AnyAsync(u => u.Email == normalizedEmail, ct);
        if (existingUser)
        {
            throw new AppException("An account with this email address already exists.", 409);
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = _hasher.Hash(request.Password),
            Role = UserRole.Candidate,
            IsEmailVerified = true // Frictionless onboarding
        };

        var profile = new UserProfile
        {
            UserId = user.Id,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Country = request.Country.Trim(),
            City = request.City.Trim(),
            PreferredCurrency = request.Country.Equals("India", StringComparison.OrdinalIgnoreCase) ? "INR" : "USD"
        };

        user.Profile = profile;

        var rawRefreshToken = _jwt.GenerateRefreshToken();
        var refreshTokenHash = _jwt.HashToken(rawRefreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _db.Users.Add(user);
        _db.RefreshTokens.Add(refreshTokenEntity);
        await _db.SaveChangesAsync(ct);

        var accessToken = _jwt.GenerateAccessToken(user);

        var authResponse = new AuthResponse(
            accessToken,
            rawRefreshToken,
            DateTime.UtcNow.AddMinutes(60),
            MapToDto(user, profile)
        );

        return ApiResponse<AuthResponse>.Ok(authResponse, "Registration successful.");
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var validationResult = await _loginValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var failures = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Common.Exceptions.ValidationException(failures);
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.DeletedAt == null, ct);

        if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !_hasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        var rawRefreshToken = _jwt.GenerateRefreshToken();
        var refreshTokenHash = _jwt.HashToken(rawRefreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _db.RefreshTokens.Add(refreshTokenEntity);
        await _db.SaveChangesAsync(ct);

        var accessToken = _jwt.GenerateAccessToken(user);

        var authResponse = new AuthResponse(
            accessToken,
            rawRefreshToken,
            DateTime.UtcNow.AddMinutes(60),
            MapToDto(user, user.Profile)
        );

        return ApiResponse<AuthResponse>.Ok(authResponse, "Login successful.");
    }

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new UnauthorizedException("Refresh token is required.");
        }

        var tokenHash = _jwt.HashToken(refreshToken);

        var tokenRecord = await _db.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.Profile)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);

        if (tokenRecord == null || !tokenRecord.IsActive || tokenRecord.User.DeletedAt != null)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        // Token Rotation: revoke current token
        var newRawRefreshToken = _jwt.GenerateRefreshToken();
        var newRefreshTokenHash = _jwt.HashToken(newRawRefreshToken);

        tokenRecord.RevokedAt = DateTime.UtcNow;
        tokenRecord.ReplacedByTokenHash = newRefreshTokenHash;

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = tokenRecord.UserId,
            TokenHash = newRefreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _db.RefreshTokens.Add(newRefreshTokenEntity);
        await _db.SaveChangesAsync(ct);

        var accessToken = _jwt.GenerateAccessToken(tokenRecord.User);

        var authResponse = new AuthResponse(
            accessToken,
            newRawRefreshToken,
            DateTime.UtcNow.AddMinutes(60),
            MapToDto(tokenRecord.User, tokenRecord.User.Profile)
        );

        return ApiResponse<AuthResponse>.Ok(authResponse, "Token refreshed successfully.");
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return ApiResponse<bool>.Ok(true, "Token revoked.");
        }

        var tokenHash = _jwt.HashToken(refreshToken);
        var tokenRecord = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);
        if (tokenRecord != null && tokenRecord.IsActive)
        {
            tokenRecord.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
        }

        return ApiResponse<bool>.Ok(true, "Token revoked.");
    }

    public async Task<ApiResponse<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null, ct);

        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        return ApiResponse<UserDto>.Ok(MapToDto(user, user.Profile));
    }

    private static UserDto MapToDto(User user, UserProfile? profile)
    {
        UserProfileDto? profileDto = null;
        if (profile != null)
        {
            profileDto = new UserProfileDto(
                profile.FirstName,
                profile.LastName,
                profile.Headline,
                profile.PhoneNumber,
                profile.Country,
                profile.City,
                profile.LinkedInUrl,
                profile.GitHubUrl,
                profile.PortfolioUrl,
                profile.PreferredCurrency
            );
        }

        return new UserDto(
            user.Id,
            user.Email,
            user.Role.ToString(),
            user.IsEmailVerified,
            profileDto
        );
    }
}
