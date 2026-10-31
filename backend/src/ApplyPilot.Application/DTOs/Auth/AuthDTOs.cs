namespace ApplyPilot.Application.DTOs.Auth;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Country,
    string City
);

public record LoginRequest(
    string Email,
    string Password
);

public record GoogleAuthRequest(
    string IdToken
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User
);

public record UserDto(
    Guid Id,
    string Email,
    string Role,
    bool IsEmailVerified,
    UserProfileDto? Profile
);

public record UserProfileDto(
    string FirstName,
    string LastName,
    string? Headline,
    string? PhoneNumber,
    string Country,
    string City,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    string PreferredCurrency
);

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string? Headline,
    string? PhoneNumber,
    string Country,
    string City,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    string PreferredCurrency
);

