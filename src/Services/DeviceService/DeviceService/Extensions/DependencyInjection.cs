// Ignore Spelling: Grpc

using BuildingBlocks.Presentation.Extensions;
using Device.Application.Extesions;
using Device.Infrastructure.Extensions;

namespace Device.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGlobalApi(configuration);
        services.AddControllers();
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);
        services.AddGrpc();

        return services;
    }
}
