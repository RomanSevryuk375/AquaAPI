// Ignore Spelling: Validator

using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;
using Contracts.gRPC.Devices;
using Control.Domain.Interfaces;
using Grpc.Core;

namespace Control.Infrastructure.GrpcClients;

public sealed class HardwareValidator(DeviceIntegrationGrpc.DeviceIntegrationGrpcClient client)
    : IHardwareValidator
{
    public async Task<Result> ValidateAssignmentAsync(
        Guid sensorId,
        Guid relayId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new ValidateHardwareAssignmentRequest
            {
                SensorId = sensorId.ToString(),
                RelayId = relayId.ToString()
            };

            ValidateHardwareAssignmentResponse response = await client.ValidateHardwareAssignmentAsync(
                request, cancellationToken: cancellationToken);
            if (!response.IsValid)
            {
                return Result.Failure(Error.Conflict(
                    ErrorCodes.Hardware.Mismatch,
                    string.Format(ErrorMessages.Hardware.MismatchFormat, sensorId, relayId)));
            }

            return Result.Success();
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return Result.Failure(Error.NotFound(
                ErrorCodes.Device.NotFound,
                string.Format(ErrorMessages.Hardware.NotFoundFormat, sensorId, relayId)));
        }
        catch (RpcException ex)
        {
            return Result.Failure(Error.Failure(
                ErrorCodes.Grpc.Error,
                string.Format(ErrorMessages.Grpc.CommunicationFailedFormat, ex.Status.Detail)));
        }
    }
}
