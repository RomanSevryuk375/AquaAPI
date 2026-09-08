using BuildingBlocks.Infrastructure.Extensions;
using Control.API.Extensions;

await MicroserviceRunner.RunAsync("AquaSmart.ControlService", args, builder =>
{
    builder.Services.AddConfiguration(builder.Configuration);
});

public partial class Program
{
    protected Program() { }
}
