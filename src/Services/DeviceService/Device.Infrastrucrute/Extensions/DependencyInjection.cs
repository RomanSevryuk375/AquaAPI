using Device.Domain.Interfaces;
using Device.Infrastructure.BackgroundJobs;
using Device.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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

    public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = new JobKey(nameof(CheckOfflineControllersJob));

            options.AddJob<CheckOfflineControllersJob>(jobOptions =>
                jobOptions.WithIdentity(jobKey));

            options.AddTrigger(triggerOptions => triggerOptions
            .ForJob(jobKey)
            .WithIdentity($"{jobKey}-trigger")
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(300).RepeatForever()));
        });

        services.AddQuartzHostedService(hostOptions     
            => hostOptions.WaitForJobsToComplete = true);

        return services;
    }
}
