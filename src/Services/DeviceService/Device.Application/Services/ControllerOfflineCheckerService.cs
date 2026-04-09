using Contracts.Events.ControllerEvents;
using Device.Application.Interfaces;
using Device.Domain.Interfaces;
using Device.Domain.Specifications;
using MassTransit;

namespace Device.Application.Services;

public class ControllerOfflineCheckerService(
    IPublishEndpoint publishEndpoint,
    IControllerRepository repository,
    IUnitOfWork unitOfWork) : IControllerOfflineCheckerService
{
    public async Task CheckAndDisableController(CancellationToken cancellationToken)
    {
        var offlineThreshold = DateTime.UtcNow.AddMinutes(-5);
        var specification = new OfflineControllersSpecification(offlineThreshold);

        var controllers = await repository.GetAllAsync(
            specification,
            null,
            null,
            cancellationToken);

        if (controllers.Any())
        {
            return;
        }

        foreach (var controller in controllers)
        {
            controller.SetOffline();
            await repository.UpdateAsync(controller, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var controller in controllers)
        {
            await publishEndpoint.Publish(new ControllerNotOnlineEvent
            {
                ControllerId = controller.Id,
                LastSeenAt = controller.LastSeenAt,
            }, cancellationToken);
        }
    }
}
