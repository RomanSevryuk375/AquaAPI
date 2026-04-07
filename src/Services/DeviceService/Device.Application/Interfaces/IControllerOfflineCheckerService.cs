namespace Device.Application.Interfaces;

public interface IControllerOfflineCheckerService
{
    Task CheckAndDisableController(
        CancellationToken cancellationToken);
}
