using BuildingBlocks.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGlobalBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }
}
