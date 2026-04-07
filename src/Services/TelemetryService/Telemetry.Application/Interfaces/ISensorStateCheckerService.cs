namespace Telemetry.Application.Interfaces;

public interface ISensorStateCheckerService
{
    Task CheckStateAndNotify(
        CancellationToken cancellationToken);
}
