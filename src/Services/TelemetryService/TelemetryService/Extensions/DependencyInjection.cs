// Ignore Spelling: Grpc

using BuildingBlocks.GrpcContracts.Extensions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.Extensions;
using Telemetry.Application.Extensions;
using Telemetry.Infrastructure.Extensions;

namespace Telemetry.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGlobalApi(configuration);
        services.AddEndpointsApiExplorer();
        services.AddEndpoints(typeof(DependencyInjection).Assembly);
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);
        services.AddDeviceGrpcClient(configuration);

        return services;
    }
}
