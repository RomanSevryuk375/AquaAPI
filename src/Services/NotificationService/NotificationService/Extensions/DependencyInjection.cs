using Contracts.Authorization;
using Notification.Application.Extensions;
using Notification.Infrastructure.Extensions;

namespace Notification.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();

        services.AddCommonAuthentication(configuration);
        services.AddServices();

        services.AddRepositories(configuration);
        services.AddQuartzJobs();
        services.AddRabbitMq(configuration);

        services.AddAquaAuthorizationPolicies();

        return services;
    }
}
