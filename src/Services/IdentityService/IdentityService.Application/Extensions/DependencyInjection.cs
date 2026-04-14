using IdentityService.Application.Interfaces;
using IdentityService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
