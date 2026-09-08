// Ignore Spelling: Grpc

using BuildingBlocks.Domain.Constants;
using Contracts.gRPC.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.GrpcContracts.Extensions;

public static class GrpcClientExtensions
{
    public static IServiceCollection AddDeviceGrpcClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        GrpcOptions? grpcOptions = configuration
            .GetSection(GrpcOptions.SectionName)
            .Get<GrpcOptions>();

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
