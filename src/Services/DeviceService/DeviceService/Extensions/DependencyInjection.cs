using Contracts.Authorization;
using Device.Application.Extesions;
using Device.Infrastructure.Extensions;

namespace Device.API.Extensions;

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

