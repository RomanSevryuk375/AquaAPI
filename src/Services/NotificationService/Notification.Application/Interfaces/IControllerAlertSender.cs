using Contracts.Events.ControllerEvents;

namespace Notification.Application.Interfaces;

public interface IControllerAlertSender
{
    Task SendControllerNotOnlineAlert(
        ControllerNotOnlineEvent controllerEvent, 
        CancellationToken cancellationToken);
}