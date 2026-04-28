using Contracts.Options;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Telemetry.Domain.Interfaces;
using Telemetry.Infrastructure.BackgroundJobs;
using Telemetry.Infrastructure.Messaging;
using Telemetry.Infrastructure.Repositories;

namespace Telemetry.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<ITelemetryDataRepository, TelemetryRawDataRepository>();

        var connectionString = configuration.GetConnectionString(nameof(SystemDbContext));
        services.AddDbContext<SystemDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddHealthChecks().AddNpgSql(connectionString!);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddQuartzJob(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = new JobKey(nameof(CheckSensorStateJob));

            options.AddJob<CheckSensorStateJob>(jobOptions =>
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

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitSection = configuration.GetSection(RabbitMqOptions.SectionName);
        var rabbitOptions = rabbitSection.Get<RabbitMqOptions>()
            ?? throw new InvalidOperationException("RabbitMQ configuration is missing.");

        services.Configure<RabbitMqOptions>(rabbitSection);

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<SensorCreatedConsumer>();
            busConfigurator.AddConsumer<SensorUpdatedConsumer>();
            busConfigurator.AddConsumer<SensorDeletedConsumer>();
            busConfigurator.AddConsumer<SensorStateChangedConsumer>();

            busConfigurator.AddConsumer<TelemetryReportedFromHardwareConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(rabbitOptions.Host), h =>
                {
                    h.Username(rabbitOptions.UserName);
                    h.Password(rabbitOptions.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddHealthChecks()
            .AddRabbitMQ(new Uri(rabbitOptions.Host));

        return services;
    }
}
