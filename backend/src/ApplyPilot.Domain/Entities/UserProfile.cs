using ApplyPilot.Domain.Common;

namespace ApplyPilot.Domain.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Headline { get; set; }
    public string? PhoneNumber { get; set; }
    public string Country { get; set; } = "India";
    public string City { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public string PreferredCurrency { get; set; } = "INR";
}

