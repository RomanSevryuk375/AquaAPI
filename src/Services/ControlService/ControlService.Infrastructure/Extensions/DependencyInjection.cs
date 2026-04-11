using Control.Domain.Interfaces;
using Control.Infrastructure.Messaging.Relay;
using Control.Infrastructure.Messaging.Sensor;
using Control.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Control.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories (this IServiceCollection services)
    {
        services.AddScoped<IAquariumRepository, AquariumRepository>();
        services.AddScoped<IAutomationRuleRepository, AutomationRuleRepository>();
        services.AddScoped<IRelayRepository, RelayRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IVacationModeRepository, VacationModeRepository>();

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<RelayCreatedEventConsumer>();
            busConfigurator.AddConsumer<RelayDeletedEventConsumer>();
            busConfigurator.AddConsumer<RelayModeChangedComandConsumer>();
            busConfigurator.AddConsumer<RelayStateChangedComandConsumer>();
            busConfigurator.AddConsumer<RelayUpdatedEventConsumer>();

            busConfigurator.AddConsumer<SensorCreatedEventconsumer>();
            busConfigurator.AddConsumer<SensorDeletedEventConsume>();
            busConfigurator.AddConsumer<SensorNoDataEventConsumer>();
            busConfigurator.AddConsumer<SensorStateChangedComandConsumer>();
            busConfigurator.AddConsumer<SensorUpdatedEventConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:UserName"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
