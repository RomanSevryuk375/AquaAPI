using System.Data;

namespace BuildingBlocks.Domain.Abstractions;

public interface ISqlConnectionFactory
{
    public IDbConnection CreateConnection();
}
