using System.Data;
using BuildingBlocks.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BuildingBlocks.Infrastructure.Data;

public sealed class SqlConnectionFactory<TDbContext>(IConfiguration configuration) : ISqlConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString(typeof(TDbContext).Name)
            ?? throw new InvalidOperationException($"ConnectionString for '{typeof(TDbContext).Name}' not found.");

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
