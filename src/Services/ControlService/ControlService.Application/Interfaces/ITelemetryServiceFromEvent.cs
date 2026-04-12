using Contracts.Events.TelemetryEvents;

namespace Control.Application.Interfaces;

public interface ITelemetryServiceFromEvent
{
    Task ProcessTelemetryAsync(
        TelemetryReceivedEvent telemetry, 
        CancellationToken cancellationToken);
}