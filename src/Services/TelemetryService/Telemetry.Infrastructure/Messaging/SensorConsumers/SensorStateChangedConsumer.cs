using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using MediatR;
using Telemetry.Application.Features.Sensors.Commands.SyncSensorStateChanged;

namespace Telemetry.Infrastructure.Messaging.SensorConsumers;

internal sealed class SensorStateChangedConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorStateChangedEvent, SyncSensorStateChangedCommand>(sender, mapper);
