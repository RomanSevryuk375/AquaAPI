using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.IntegrationEvents.Events.Telemetrys;

namespace Telemetry.Application.Features.Telemetry.Commands.AddTelemetryBatch;

public sealed record AddTelemetryBatchCommand : ICommand
{
    public string MacAddress { get; init; } = string.Empty;
    public string DeviceToken { get; init; } = string.Empty;
    public IReadOnlyList<TelemetryBatchEventItem> Items { get; init; } = [];
}
