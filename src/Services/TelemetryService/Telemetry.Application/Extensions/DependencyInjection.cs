using Microsoft.Extensions.DependencyInjection;
using Telemetry.Application.Interfaces;
using Telemetry.Application.Services;

namespace Telemetry.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISensorService, SensorService>();
        services.AddScoped<ITelemetryDataService, TelemetryDataService>();

        return services;
    }
}
