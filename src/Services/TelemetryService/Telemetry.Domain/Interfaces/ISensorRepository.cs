namespace Telemetry.Domain.Interfaces;

public interface ISensorRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
