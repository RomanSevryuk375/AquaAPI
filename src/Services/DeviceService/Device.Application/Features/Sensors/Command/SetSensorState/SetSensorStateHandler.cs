using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Sensors.Command.SetSensorState;

public sealed class SetSensorStateHandler(ISensorRepository sensorRepository)
    : IRequestHandler<SetSensorStateCommand, Result>
{
    public async Task<Result> Handle(
        SetSensorStateCommand request,
        CancellationToken cancellationToken)
    {
        Sensor? existingSensor = await sensorRepository.GetByIdAsync(
            request.SensorId, cancellationToken);

        existingSensor!.SetState(request.SensorState);

        return Result.Success();
    }
}
