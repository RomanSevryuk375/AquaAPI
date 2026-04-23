using Contracts.JwtToken;
using IdentityService.Application.Extensions;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityDbContext)))
                .UseSnakeCaseNamingConvention();
        });

        services.AddRabbitMq(configuration);

        services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IdentityDbContext>() 
        .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddServices();

        return services;
    }
}
