using Control.Domain.Interfaces;
using Control.Infrastructure.BackgroundJobs;
using Control.Infrastructure.Messaging.Relay;
using Control.Infrastructure.Messaging.Sensor;
using Control.Infrastructure.Messaging.Telemetry;
using Control.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddDelayedMessageScheduler();
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

            busConfigurator.AddConsumer<TelemetryReceivedEventConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.UseDelayedMessageScheduler();

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

    public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = new JobKey(nameof(ScheduleProcessJob));

            options.AddJob<ScheduleProcessJob>(jobOptions =>
                jobOptions.WithIdentity(jobKey));

            options.AddTrigger(triggerOptions => triggerOptions
                .ForJob(jobKey)
                .WithIdentity($"{jobKey}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever()));
        });

        services.AddQuartzHostedService(hostOptions
            => hostOptions.WaitForJobsToComplete = true);

        return services;
    }
}
