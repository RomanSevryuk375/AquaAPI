using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.BackgroundJob;
using Notification.Infrastructure.Messaging;
using Notification.Infrastructure.Providers;
using Notification.Infrastructure.Repositories;
using Quartz;

namespace Notification.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.SectionName));
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services.AddHttpClient<INotificationProvider, TgProvider>();
        services.AddSingleton<INotificationProvider, EmailProvider>();

        services.AddScoped<IAquariumRepository, AquariumRepository>();
        services.AddScoped<IMaintenanceLogRepository, MaintenanceLogRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<AquariumCreatedEventConsumer>();
            busConfigurator.AddConsumer<AquariumDeletedEventConsumer>();
            busConfigurator.AddConsumer<AquariumUpdatedEventConsumer>();

            busConfigurator.AddConsumer<UserCreatedEventConsumer>();

            busConfigurator.AddConsumer<CriticalTelemetryThresholdAlertEventConsumer>();

            busConfigurator.AddConsumer<SensorNoDataAlertEventConsumer>();

            busConfigurator.AddConsumer<ControllerNotOnlineEventConsumer>();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value; 

                configurator.Host(new Uri(options.Host), h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
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
            var reminderJobKey = new JobKey(nameof(ReminderCheckerJob));

            var unpublishedNoticeJobKey = new JobKey(nameof(UnpublishedNoticeProcessorJob));

            options.AddJob<ReminderCheckerJob>(jobOptions =>
                jobOptions.WithIdentity(reminderJobKey));

            options.AddJob<UnpublishedNoticeProcessorJob>(jobOptions =>
                jobOptions.WithIdentity(unpublishedNoticeJobKey));

            options.AddTrigger(triggerOptions => triggerOptions
                .ForJob(reminderJobKey)
                .WithIdentity($"{reminderJobKey}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever()));

            options.AddTrigger(triggerOptions => triggerOptions
                .ForJob(unpublishedNoticeJobKey)
                .WithIdentity($"{unpublishedNoticeJobKey}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever()));
        });

        services.AddQuartzHostedService(hostOptions
            => hostOptions.WaitForJobsToComplete = true);

        return services;
    }
}
