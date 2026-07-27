using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;
using Telemetry.Infrastructure.Persistence;
using Telemetry.TestShared.Constants;
using Testcontainers.PostgreSql;

namespace Telemetry.Infrastructure.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string PostgresImage = "postgres:16-alpine";
    private const string DatabaseName = "telemetry_test_db";
    private const string Username = "postgres";
    private const string Password = "postgres";

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

        builder.UseSetting($"ConnectionStrings:{nameof(TelemetryDbContext)}", _dbContainer.GetConnectionString());

        builder.ConfigureServices(services =>
        {
            services.AddScoped<BuildingBlocks.Infrastructure.Data.Outbox.OutboxMessageProcessorService<TelemetryDbContext>>();

            var massTransitAssembly = typeof(IBus).Assembly;
            var massTransitDescriptors = services.Where(d =>
                d.ServiceType.Namespace?.StartsWith("MassTransit") == true ||
                d.ImplementationType?.Namespace?.StartsWith("MassTransit") == true ||
                d.ServiceType.Assembly == massTransitAssembly ||
                d.ImplementationType?.Assembly == massTransitAssembly ||
                d.ImplementationFactory?.Method.DeclaringType?.Assembly == massTransitAssembly ||
                d.ImplementationFactory?.Method.ReturnType.Assembly == massTransitAssembly ||
                d.ServiceType.FullName?.Contains("MassTransit") == true ||
                d.ImplementationType?.FullName?.Contains("MassTransit") == true ||
                (d.ImplementationType != null && d.ImplementationType.Name.Contains("MassTransit"))).ToList();

            foreach (ServiceDescriptor descriptor in massTransitDescriptors)
            {
                services.Remove(descriptor);
            }

            services.PostConfigure<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckServiceOptions>(options =>
            {
                var uniqueRegistrations = options.Registrations
                    .GroupBy(r => r.Name)
                    .Select(g => g.First())
                    .ToList();
                options.Registrations.Clear();
                foreach (var reg in uniqueRegistrations)
                {
                    options.Registrations.Add(reg);
                }
            });

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

            services.AddSingleton(Substitute.For<ITelemetryNotifier>());

            IDeviceTokenValidator tokenValidatorMock = Substitute.For<IDeviceTokenValidator>();
            tokenValidatorMock.ValidateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Result<ValidateResponseDto>.Success(new ValidateResponseDto { ControllerId = TestConstants.ControllerId, UserId = TestConstants.UserId }));
            services.AddSingleton(tokenValidatorMock);

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
            TelemetryDbContext context = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();
            context.Database.Migrate();
        });
    }
}

public sealed class TestUserContext : IUserContext
{
    public Guid UserId { get; set; } = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public bool IsAuthenticated { get; set; } = true;
}

