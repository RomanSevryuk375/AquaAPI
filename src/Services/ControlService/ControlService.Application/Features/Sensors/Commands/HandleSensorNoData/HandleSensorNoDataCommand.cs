using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;

namespace Control.Application.Features.Sensors.Commands.HandleSensorNoData;

public sealed record HandleSensorNoDataCommand : ICommand
{
    public Guid SensorId { get; init; }
    public SensorState State { get; init; }
    public DateTime LastSeenAt { get; init; }
}
