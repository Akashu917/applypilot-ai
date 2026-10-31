using ApplyPilot.Domain.Common;
using ApplyPilot.Domain.Enums;

namespace ApplyPilot.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.Candidate;
    public bool IsEmailVerified { get; set; } = false;
    public string? EmailVerificationToken { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpiresAt { get; set; }
    public string? GoogleId { get; set; }
    public DateTime? DeletedAt { get; set; }

    public UserProfile? Profile { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

