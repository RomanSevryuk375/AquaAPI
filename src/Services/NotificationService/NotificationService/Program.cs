using BuildingBlocks.Infrastructure.Extensions;
using Notification.API.Extensions;

await MicroserviceRunner.RunAsync("AquaSmart.NotificationService", args, builder =>
{
    builder.Services.AddConfiguration(builder.Configuration);
});

public partial class Program
{
    protected Program() { }
}
