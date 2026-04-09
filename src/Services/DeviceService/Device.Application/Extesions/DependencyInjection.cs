using Device.Application.Interfaces;
using Device.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Device.Application.Extesions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices (this IServiceCollection services)
    {
        services.AddScoped<IControllerOfflineCheckerService, ControllerOfflineCheckerService>();
        services.AddScoped<IControllerService, ControllerService>();
        services.AddScoped<IRelayService, RelayService>();
        services.AddScoped<ISensorService, SensorService>();

        return services;
    }
}
