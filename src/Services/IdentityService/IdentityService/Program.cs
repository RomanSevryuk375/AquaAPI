using BuildingBlocks.Infrastructure.Extensions;
using IdentityService.API.Extensions;

await MicroserviceRunner.RunAsync("AquaSmart.IdentityService", args, builder =>
{
    builder.Services.AddConfiguration(builder.Configuration);
});

public partial class Program
{
    protected Program() { }
}
