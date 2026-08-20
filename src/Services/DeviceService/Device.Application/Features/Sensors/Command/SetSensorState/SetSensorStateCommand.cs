
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using Device.Application.Interfaces;

namespace Device.Application.Features.Sensors.Command.SetSensorState;

public sealed record SetSensorStateCommand
    : ICommand, ISensorBoundRequest
{
    public Guid UserId { get; init; }
    public Guid SensorId { get; init; }
    public SensorState SensorState { get; init; }
}
