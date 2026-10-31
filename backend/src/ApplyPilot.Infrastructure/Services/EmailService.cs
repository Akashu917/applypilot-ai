using ApplyPilot.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApplyPilot.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendWelcomeEmailAsync(string toEmail, string firstName)
    {
        _logger.LogInformation("Sending welcome email to {Email} for candidate {Name}", toEmail, firstName);
        return Task.CompletedTask;
    }

    public Task SendVerificationEmailAsync(string toEmail, string token)
    {
        _logger.LogInformation("Sending verification token to {Email}", toEmail);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetEmailAsync(string toEmail, string token)
    {
        _logger.LogInformation("Sending password reset link to {Email}", toEmail);
        return Task.CompletedTask;
    }
}

