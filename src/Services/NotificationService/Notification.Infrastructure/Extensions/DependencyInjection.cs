using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.BackgroundJob;
using Notification.Infrastructure.Messaging;
using Notification.Infrastructure.Repositories;
using Quartz;

namespace Notification.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAquariumRepository, AquariumRepository>();
        services.AddScoped<IMaintenanceLogRepository, MaintenanceLogRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddConsumer<AquariumCreatedEventConsumer>();
            busConfigurator.AddConsumer<AquariumDeletedEventConsumer>();
            busConfigurator.AddConsumer<AquariumUpdatedEventConsumer>();

            busConfigurator.AddConsumer<UserCreatedEventConsumer>();

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

    public static IServiceCollection AddQuartzJob(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var reminderJobKey = new JobKey(nameof(RemienderProcessorJob));

            var unpublishedNoticeJobKey = new JobKey(nameof(UnpublishedNoticeProcessorJob));

            options.AddJob<RemienderProcessorJob>(jobOptions =>
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

        return services;
    }
}
