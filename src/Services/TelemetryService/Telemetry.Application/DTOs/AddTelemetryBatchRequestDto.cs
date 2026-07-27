// Ignore Spelling: Dto

using BuildingBlocks.IntegrationEvents.Events.Telemetrys;

namespace Telemetry.Application.DTOs;

public sealed record AddTelemetryBatchRequestDto
{
    public string MacAddress { get; init; } = string.Empty;
    public List<TelemetryBatchEventItem> Items { get; init; } = [];
}
