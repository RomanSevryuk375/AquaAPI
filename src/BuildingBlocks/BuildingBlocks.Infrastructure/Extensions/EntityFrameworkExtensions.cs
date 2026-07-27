// Ignore Spelling: Postgres

using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Infrastructure.Data;
using BuildingBlocks.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class EntityFrameworkExtensions
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration) where TDbContext : DbContext
    {
        string? connectionString = configuration.GetConnectionString(typeof(TDbContext).Name);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{typeof(TDbContext).Name}' is missing.");
        }

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            ConvertDomainEventsToOutboxMessagesInterceptor interceptor =
                sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention()
                   .AddInterceptors(interceptor);
        });
        services.AddHealthChecks().AddNpgSql(connectionString);
        services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
        services.AddHostedService<DatabaseMigrationService<TDbContext>>();

        return services;
    }
}

internal sealed class DatabaseMigrationService<TDbContext>(IServiceProvider serviceProvider)
    : IHostedService where TDbContext : DbContext
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
