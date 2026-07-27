using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Telemetrys;
using MediatR;
using Notification.Application.Features.Alerts.Commands.SendTelemetryAlert;

namespace Notification.Infrastructure.Messaging.Alert;

internal sealed class CriticalTelemetryThresholdAlertEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<CriticalTelemetryThresholdAlertEvent, SendTelemetryAlertCommand>(sender, mapper);

