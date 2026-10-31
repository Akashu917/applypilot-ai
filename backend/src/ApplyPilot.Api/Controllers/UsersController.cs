using ApplyPilot.Application.Common.Interfaces;
using ApplyPilot.Application.DTOs.Auth;
using ApplyPilot.Application.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplyPilot.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserProfileService _profileService;
    private readonly ICurrentUserService _currentUserService;

    public UsersController(IUserProfileService profileService, ICurrentUserService currentUserService)
    {
        _profileService = profileService;
        _currentUserService = currentUserService;
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        if (_currentUserService.UserId == null)
        {
            return Unauthorized(ApiResponse<object>.Fail("User is not authenticated."));
        }

        var result = await _profileService.GetProfileAsync(_currentUserService.UserId.Value, ct);
        return Ok(result);
    }

    [HttpPut("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        if (_currentUserService.UserId == null)
        {
            return Unauthorized(ApiResponse<object>.Fail("User is not authenticated."));
        }

        var result = await _profileService.UpdateProfileAsync(_currentUserService.UserId.Value, request, ct);
        return Ok(result);
    }
}

