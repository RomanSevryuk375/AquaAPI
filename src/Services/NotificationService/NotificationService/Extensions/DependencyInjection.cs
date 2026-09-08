// Ignore Spelling: Grpc

using BuildingBlocks.GrpcContracts.Extensions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.Extensions;
using Notification.Application.Extensions;
using Notification.Infrastructure.Extensions;

namespace Notification.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGlobalApi(configuration);
        services.AddEndpointsApiExplorer();
        services.AddEndpoints(typeof(DependencyInjection).Assembly);
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddDeviceGrpcClient(configuration);

        return services;
    }
}
