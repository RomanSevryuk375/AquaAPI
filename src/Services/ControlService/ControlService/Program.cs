using BuildingBlocks.Infrastructure.Extensions;
using BuildingBlocks.Presentation.Extensions;
using Control.API.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

string appName = "AquaSmart.ControlService";

try
{
    Log.Information("Starting {Name}", appName);

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.AddGlobalElkLogging(appName);
    builder.Services.AddConfiguration(builder.Configuration);

    WebApplication app = builder.Build();

    app.AddGlobalConfiguration();

    await app.RunAsync();
}
#pragma warning disable S2139 
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "{Name} terminated unexpectedly", appName);
    throw;
}
#pragma warning restore S2139
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
    protected Program() { }
}