using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Telemetrys;
using Control.Application.Features.Telemetry.Commands.ProcessTelemetry;
using MediatR;

namespace Control.Infrastructure.Messaging.Telemetry;

internal sealed class TelemetryReceivedEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<TelemetryReceivedEvent, ProcessTelemetryCommand>(sender, mapper);

