using System.Text;
using ApplyPilot.Application.Common.Interfaces;
using ApplyPilot.Infrastructure.Persistence;
using ApplyPilot.Infrastructure.Security;
using ApplyPilot.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ApplyPilot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConn = configuration.GetConnectionString("PostgreSQL");

        if (!string.IsNullOrWhiteSpace(postgresConn))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(postgresConn, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }
        else
        {
            var sqliteConn = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=applypilot.db";
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(sqliteConn, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();

        var jwtSecret = configuration["Jwt:Secret"] ?? "ApplyPilotAI_SuperSecretKey_ProductionGrade_2026_Minimum256BitsRequired!";
        var jwtIssuer = configuration["Jwt:Issuer"] ?? "ApplyPilot.Api";
        var jwtAudience = configuration["Jwt:Audience"] ?? "ApplyPilot.Client";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}

