using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BuildingBlocks.Infrastructure.Data.Outbox;

public static class OutboxExtension
{
    public static IServiceCollection AddOutboxProcessorQuartzJob<TDbContext>(
    this IServiceCollection services,
    int intervalSeconds = 1) where TDbContext : DbContext
    {
        services.AddScoped<OutboxMessageProcessorService<TDbContext>>();

        services.AddQuartz(opts =>
        {
            var outboxKey = new JobKey($"OutboxMessageProcessorJob-{typeof(TDbContext).Name}");
            opts.AddJob<OutboxMessageProcessorJob<TDbContext>>(opts => opts.WithIdentity(outboxKey));
            opts.AddTrigger(trigger => trigger
                .ForJob(outboxKey)
                .WithIdentity($"{outboxKey}-trigger")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalSeconds).RepeatForever()));
        });

        return services;
    }
}
