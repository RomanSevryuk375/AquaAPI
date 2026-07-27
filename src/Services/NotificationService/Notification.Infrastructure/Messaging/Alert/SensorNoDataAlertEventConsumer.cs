using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using MediatR;
using Notification.Application.Features.Alerts.Commands.SendSensorNoDataAlert;

namespace Notification.Infrastructure.Messaging.Alert;

internal sealed class SensorNoDataAlertEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorNoDataAlertEvent, SendSensorNoDataAlertCommand>(sender, mapper);

