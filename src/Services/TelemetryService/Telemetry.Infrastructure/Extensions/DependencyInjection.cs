using Microsoft.Extensions.DependencyInjection;
using Telemetry.Domain.Interfaces;
using Telemetry.Infrastructure.Repositories;

namespace Telemetry.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<ITelemetryDataRepository, TelemetryDataRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
