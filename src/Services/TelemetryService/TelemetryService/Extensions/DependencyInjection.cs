// Ignore Spelling: Grpc

using BuildingBlocks.Domain.Constants;
using BuildingBlocks.GrpcContracts;
using BuildingBlocks.Presentation.Extensions;
using Contracts.gRPC.Devices;
using Telemetry.Application.Extensions;
using Telemetry.Infrastructure.Extensions;

namespace Telemetry.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGlobalApi(configuration);
        services.AddControllers();
        services.AddApplication(configuration);
        services.AddInfrastructure(configuration);
        services.AddMyGrpcClient(configuration);

        return services;
    }

    private static IServiceCollection AddMyGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        GrpcOptions? grpcOptions = configuration.GetSection(GrpcOptions.SectionName).Get<GrpcOptions>();
        if (grpcOptions is null || string.IsNullOrWhiteSpace(grpcOptions.DeviceServiceUrl))
        {
            throw new InvalidOperationException(DiErrors.GrpcConfiguration);
        }

        services.AddGrpcClient<DeviceIntegrationGrpc.DeviceIntegrationGrpcClient>(options =>
        {
            options.Address = new Uri(grpcOptions.DeviceServiceUrl);
        });

        return services;
    }
}
