using Control.Application.Interfaces;
using Control.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Control.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAquariumService, AquariumService>();
        services.AddScoped<IAutomationRuleService, AutomationRuleService>();
        services.AddScoped<IRelayServiceFromEvent, RelayServiceFromEvent>();
        services.AddScoped<IScheduleProcessor, ScheduleProcessor>();
        services.AddScoped<ISensorServiceFromEvent, SensorServiceFromEvent>();
        services.AddScoped<ITelemetryServiceFromEvent, TelemetryServiceFromEvent>();

        return services;
    }
}