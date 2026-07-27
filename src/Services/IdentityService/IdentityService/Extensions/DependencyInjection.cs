using BuildingBlocks.Presentation.Extensions;
using IdentityService.Application.Extensions;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGlobalApi(configuration);
        services.AddControllers();
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
