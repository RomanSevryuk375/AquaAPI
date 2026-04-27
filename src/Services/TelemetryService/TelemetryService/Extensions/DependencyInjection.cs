using Contracts.Authorization;
using Telemetry.Application.Extensions;
using Telemetry.Infrastructure.Extensions;

namespace Telemetry.API.Extensions;

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
        services.AddQuartzJob();
        services.AddRabbitMq(configuration);

        services.AddAquaAuthorizationPolicies();

        return services;
    }
}
