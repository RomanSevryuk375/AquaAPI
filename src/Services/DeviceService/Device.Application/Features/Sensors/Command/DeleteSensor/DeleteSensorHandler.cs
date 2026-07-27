using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Sensors.Command.DeleteSensor;

public sealed class DeleteSensorHandler(ISensorRepository sensorRepository)
    : IRequestHandler<DeleteSensorCommand, Result>
{
    public async Task<Result> Handle(
        DeleteSensorCommand request,
        CancellationToken cancellationToken)
    {
        Sensor? existingSensor = await sensorRepository.GetByIdAsync(
            request.SensorId, cancellationToken);

        existingSensor!.MarkAsDeleted();

        await sensorRepository.DeleteAsync(request.SensorId, cancellationToken);

        return Result.Success();
    }
}
