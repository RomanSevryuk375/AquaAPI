using BuildingBlocks.Infrastructure.Extensions;
using Telemetry.API.Extensions;

await MicroserviceRunner.RunAsync("AquaSmart.TelemetryService", args, builder =>
{
    builder.Services.AddConfiguration(builder.Configuration);
});

public partial class Program
{
    protected Program() { }
}
