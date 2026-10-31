using ApplyPilot.Application.Common.Interfaces;
using ApplyPilot.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplyPilot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        return services;
    }
}

