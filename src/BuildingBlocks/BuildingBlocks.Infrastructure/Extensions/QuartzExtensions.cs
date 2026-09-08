using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzWithHostedService(
        this IServiceCollection services,
        Action<IServiceCollectionQuartzConfigurator> configure)
    {
        services.AddQuartz(configure);

        services.AddQuartzHostedService(hostOptions =>
            hostOptions.WaitForJobsToComplete = true);

        return services;
    }
}
