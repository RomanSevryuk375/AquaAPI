using Contracts.Events.TelemetryEvents;
using MassTransit;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.Messaging;

public class TelemetryReportedFromHardwareConsumer(
    ITelemetryDataService dataService) : IConsumer<TelemetryReportedFromHardwareEvent>
{
    public async Task Consume(ConsumeContext<TelemetryReportedFromHardwareEvent> context)
    {
        await dataService.AddDataAsync(
            context.Message, context.CancellationToken);
    }
}
