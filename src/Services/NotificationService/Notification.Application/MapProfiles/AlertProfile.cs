using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Controllers;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using BuildingBlocks.IntegrationEvents.Events.Telemetrys;
using BuildingBlocks.IntegrationEvents.Events.Users;
using Notification.Application.Features.Alerts.Commands.SendControllerOfflineAlert;
using Notification.Application.Features.Alerts.Commands.SendSensorNoDataAlert;
using Notification.Application.Features.Alerts.Commands.SendSubscriptionAlert;
using Notification.Application.Features.Alerts.Commands.SendTelemetryAlert;

namespace Notification.Application.MapProfiles;

public sealed class AlertProfile : Profile
{
    public AlertProfile()
    {
        CreateMap<ControllerNotOnlineEvent, SendControllerOfflineAlertCommand>();

        CreateMap<SensorNoDataAlertEvent, SendSensorNoDataAlertCommand>();

        CreateMap<CriticalTelemetryThresholdAlertEvent, SendTelemetryAlertCommand>();

        CreateMap<SubscriptionDowngradedEvent, SendSubscriptionAlertCommand>();
    }
}
