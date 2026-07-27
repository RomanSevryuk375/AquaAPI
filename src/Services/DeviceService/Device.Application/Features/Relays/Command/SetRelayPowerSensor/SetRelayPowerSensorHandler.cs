using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Relays.Command.SetRelayPowerSensor;

public sealed class SetRelayPowerSensorHandler(
    IRelayRepository relayRepository,
    ISensorRepository sensorRepository) : IRequestHandler<SetRelayPowerSensorCommand, Result>
{
    public async Task<Result> Handle(
        SetRelayPowerSensorCommand request,
        CancellationToken cancellationToken)
    {
        Relay? existingRelay = await relayRepository.GetByIdAsync(
            request.RelayId, cancellationToken);

        Sensor? powerSensor = await sensorRepository.GetByIdAsync(
            request.PowerSensorId, cancellationToken);
        if (powerSensor is null)
        {
            return Result.Failure(Error.NotFound<VoltageSensor>(
                    string.Format(ErrorMessages.SensorNotFound, request.PowerSensorId)));
        }

        if (existingRelay!.ControllerId != powerSensor.ControllerId)
        {
            return Result.Failure(Error.Validation<Relay>(
                ErrorMessages.SensorAndRelaySameController));
        }

        existingRelay.SetPowerSensor(powerSensor);

        return Result.Success();
    }
}
