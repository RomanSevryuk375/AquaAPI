using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Testcontainers.PostgreSql;

namespace Control.Infrastructure.IntegrationTests.Infrastructure;

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

        builder.UseSetting("ConnectionStrings:ControlDbContext", _dbContainer.GetConnectionString());

        builder.UseSetting("JwtOptions:SecretKey", "test-secret-key-must-be-at-least-32-characters-long");
        builder.UseSetting("JwtOptions:Issuer", "AquaSmart.Identity");
        builder.UseSetting("JwtOptions:Audience", "AquaSmart.Gateway");
        builder.UseSetting("JwtOptions:ExpiresHours", "12");

        builder.ConfigureServices(services =>
        {
            var massTransitDescriptors = services.Where(d =>
                d.ServiceType.Namespace?.StartsWith("MassTransit") is true ||
                d.ImplementationType?.Namespace?.StartsWith("MassTransit") is true).ToList();

            foreach (ServiceDescriptor? descriptor in massTransitDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddMassTransit(x =>
            {
                x.AddDelayedMessageScheduler();
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                });
            });

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

            IHardwareValidator hardwareValidatorMock = Substitute.For<IHardwareValidator>();
            hardwareValidatorMock.ValidateAssignmentAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success());
            services.AddSingleton(hardwareValidatorMock);

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();
            ControlDbContext context = scope.ServiceProvider.GetRequiredService<ControlDbContext>();
            context.Database.Migrate();
        });
    }
}

public sealed class TestUserContext : IUserContext
{
    public Guid UserId { get; set; } = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public bool IsAuthenticated { get; set; } = true;
}
