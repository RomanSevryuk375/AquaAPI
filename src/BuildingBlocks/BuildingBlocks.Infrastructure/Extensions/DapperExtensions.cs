using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class DapperExtensions
{
    public static IServiceCollection AddDapper<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory<TDbContext>>();

        return services;
    }
}
