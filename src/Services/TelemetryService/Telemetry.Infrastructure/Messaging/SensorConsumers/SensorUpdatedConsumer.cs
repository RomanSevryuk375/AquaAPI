using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using MediatR;
using Telemetry.Application.Features.Sensors.Commands.SyncSensorUpdated;

namespace Telemetry.Infrastructure.Messaging.SensorConsumers;

internal sealed class SensorUpdatedConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorUpdatedEvent, SyncSensorUpdatedCommand>(sender, mapper);
