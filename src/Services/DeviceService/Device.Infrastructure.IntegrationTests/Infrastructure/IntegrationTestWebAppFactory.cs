using BuildingBlocks.Domain.Abstractions;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Testcontainers.PostgreSql;

namespace Device.Infrastructure.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string PostgresImage = "postgres:16-alpine";
    private const string DatabaseName = "device_test_db";
    private const string Username = "postgres";
#pragma warning disable S2068 // Hard-coded credentials are safe in tests for Testcontainers
    private const string Password = "postgres";
#pragma warning restore S2068 // Hard-coded credentials are safe in tests for Testcontainers

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder(PostgresImage)
        .WithImage(PostgresImage)
        .WithDatabase(DatabaseName)
        .WithUsername(Username)
        .WithPassword(Password)
        .Build();

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting("ConnectionStrings:DeviceDbContext", _dbContainer.GetConnectionString());

        builder.UseSetting("JwtOptions:SecretKey", "test-secret-key-must-be-at-least-32-characters-long");
        builder.UseSetting("JwtOptions:Issuer", "AquaSmart.Identity");
        builder.UseSetting("JwtOptions:Audience", "AquaSmart.Gateway");
        builder.UseSetting("JwtOptions:ExpiresHours", "12");

        builder.ConfigureTestServices(services =>
        {
            services.AddMassTransitTestHarness(x => x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context)));

            ServiceDescriptor? quartzHostedService = services.FirstOrDefault(d =>
                d.ImplementationType?.Name == "QuartzHostedService");
            if (quartzHostedService != null)
            {
                services.Remove(quartzHostedService);
            }

            ServiceDescriptor? userContextDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(IUserContext));
            if (userContextDescriptor != null)
            {
                services.Remove(userContextDescriptor);
            }

            services.AddSingleton<TestUserContext>();
            services.AddTransient<IUserContext>(sp =>
                sp.GetRequiredService<TestUserContext>());

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();
            DeviceDbContext context = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
            context.Database.Migrate();
        });
    }
}

public sealed class TestUserContext : IUserContext
{
    public Guid UserId { get; set; } = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public bool IsAuthenticated { get; set; } = true;
}
