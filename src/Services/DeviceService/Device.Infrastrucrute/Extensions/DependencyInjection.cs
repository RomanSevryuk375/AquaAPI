using Device.Domain.Interfaces;
using Device.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Device.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories (this IServiceCollection services)
    {
        services.AddScoped<IControllerRepository, ControllerRepository>();
        services.AddScoped<IRelayRepository, RelayRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
