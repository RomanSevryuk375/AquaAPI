using Contracts.Authorization;
using Control.Application.Extensions;
using Control.Infrastructure.Extensions;

namespace Control.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();

        services.AddServices();
        services.AddRepositories(configuration);
        services.AddQuartzJobs();
        services.AddRabbitMq(configuration);

        services.AddAquaAuthorizationPolicies();

        return services;
    }
}
