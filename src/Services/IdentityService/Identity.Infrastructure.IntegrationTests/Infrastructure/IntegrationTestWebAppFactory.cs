using BuildingBlocks.IntegrationTests;
using MassTransit;
using Microsoft.AspNetCore.TestHost;

namespace Identity.Infrastructure.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : BaseIntegrationTestWebAppFactory<Program, IdentityDbContext>
{
    protected override string GetDbConnectionStringName() => "ConnectionStrings:IdentityDbContext";

    protected override void ConfigureMassTransit(IServiceCollection services)
    {
        services.AddMassTransitTestHarness(x => x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); }));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<BuildingBlocks.Infrastructure.Data.Outbox.OutboxMessageProcessorService<IdentityDbContext>>();
        });
    }
}
