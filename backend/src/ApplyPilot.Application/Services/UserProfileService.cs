using ApplyPilot.Application.Common.Exceptions;
using ApplyPilot.Application.Common.Interfaces;
using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.DTOs.Common;
using ApplyPilot.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ApplyPilot.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IAppDbContext _db;
    private readonly IValidator<UpdateProfileRequest> _validator;

    public UserProfileService(IAppDbContext db, IValidator<UpdateProfileRequest> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var profile = await _db.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile == null)
        {
            throw new NotFoundException("Profile not found.");
        }

        return ApiResponse<UserProfileDto>.Ok(MapToDto(profile));
    }

    public async Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var failures = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            throw new Common.Exceptions.ValidationException(failures);
        }

        var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, ct);
        if (profile == null)
        {
            profile = new UserProfile { UserId = userId };
            _db.UserProfiles.Add(profile);
        }

        profile.FirstName = request.FirstName.Trim();
        profile.LastName = request.LastName.Trim();
        profile.Headline = request.Headline?.Trim();
        profile.PhoneNumber = request.PhoneNumber?.Trim();
        profile.Country = request.Country.Trim();
        profile.City = request.City.Trim();
        profile.LinkedInUrl = request.LinkedInUrl?.Trim();
        profile.GitHubUrl = request.GitHubUrl?.Trim();
        profile.PortfolioUrl = request.PortfolioUrl?.Trim();
        profile.PreferredCurrency = string.IsNullOrWhiteSpace(request.PreferredCurrency) 
            ? (request.Country.Equals("India", StringComparison.OrdinalIgnoreCase) ? "INR" : "USD") 
            : request.PreferredCurrency.Trim().ToUpperInvariant();
        profile.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return ApiResponse<UserProfileDto>.Ok(MapToDto(profile), "Profile updated successfully.");
    }

    private static UserProfileDto MapToDto(UserProfile profile)
    {
        return new UserProfileDto(
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
}

