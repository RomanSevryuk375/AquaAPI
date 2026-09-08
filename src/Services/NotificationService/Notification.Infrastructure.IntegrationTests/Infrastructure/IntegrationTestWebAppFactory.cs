// Ignore Spelling: Tg

using BuildingBlocks.Domain.Abstractions;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.Persistence;
using NSubstitute;
using Testcontainers.PostgreSql;

namespace Notification.Infrastructure.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string PostgresImage = "postgres:16-alpine";
    private const string DatabaseName = "notification_test_db";
    private const string Username = "postgres";
#pragma warning disable S2068 // Hard-coded credentials are safe in tests for Testcontainers
    private const string Password = "postgres";
#pragma warning restore S2068 // Hard-coded credentials are safe in tests for Testcontainers

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder(PostgresImage)
        .WithDatabase(DatabaseName)
        .WithUsername(Username)
        .WithPassword(Password)
        .Build();

    public IEmailProvider EmailProviderMock { get; } = Substitute.For<IEmailProvider>();
    public ITgProvider TgProviderMock { get; } = Substitute.For<ITgProvider>();

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting("ConnectionStrings:NotificationDbContext", _dbContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<BuildingBlocks.Infrastructure.Data.Outbox.OutboxMessageProcessorService<NotificationDbContext>>();
            services.AddMassTransitTestHarness(x =>
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            ServiceDescriptor? quartzHostedService = services.FirstOrDefault(d =>
                d.ImplementationType?.Name == "QuartzHostedService");
            if (quartzHostedService != null)
            {
                services.Remove(quartzHostedService);
            }

            ServiceDescriptor? migrationHostedService = services.FirstOrDefault(d =>
                d.ImplementationType?.Name == "DatabaseMigrationService");
            if (migrationHostedService != null)
            {
                services.Remove(migrationHostedService);
            }

            var tgDescriptors = services.Where(d => d.ServiceType == typeof(ITgProvider)).ToList();
            foreach (ServiceDescriptor? descriptor in tgDescriptors)
            {
                services.Remove(descriptor);
            }

            var emailDescriptors = services.Where(d => d.ServiceType == typeof(IEmailProvider)).ToList();
            foreach (ServiceDescriptor? descriptor in emailDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IEmailProvider>(EmailProviderMock);
            services.AddSingleton<ITgProvider>(TgProviderMock);

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
            NotificationDbContext context = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            context.Database.Migrate();
        });
    }
}

public sealed class TestUserContext : IUserContext
{
    public Guid UserId { get; set; } = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public bool IsAuthenticated { get; set; } = true;
}
