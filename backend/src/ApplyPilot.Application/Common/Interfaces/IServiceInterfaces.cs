using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.DTOs.Common;

namespace ApplyPilot.Application.Common.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task<ApiResponse<bool>> RevokeTokenAsync(string refreshToken, CancellationToken ct = default);
    Task<ApiResponse<UserDto>> GetCurrentUserAsync(Guid userId, CancellationToken ct = default);
}

public interface IUserProfileService
{
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
}

