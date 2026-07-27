using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Device.Domain.Factories;
using MassTransit;

namespace Device.Application.Features.Sensors.Command.AddSensor;

public sealed class AddSensorHandler(
    ISensorRepository sensorRepository,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<AddSensorCommand, Result<SensorCreatedResponse>>
{
    public async Task<Result<SensorCreatedResponse>> Handle(
        AddSensorCommand request,
        CancellationToken cancellationToken)
    {
        Result<Sensor> sensor = SensorFactory.CreateSensor(
            id: NewId.NextGuid(), request.ControllerId, userContext.UserId,
            request.Name,
            request.ConnectionProtocol, request.ConnectionAddress,
            request.Type);
        if (sensor.IsFailure)
        {
            return Result<SensorCreatedResponse>.Failure(sensor.Error);
        }

        await sensorRepository.AddAsync(sensor.Value, cancellationToken);

        return Result<SensorCreatedResponse>.Success(
            mapper.Map<SensorCreatedResponse>(sensor.Value));
    }
}
