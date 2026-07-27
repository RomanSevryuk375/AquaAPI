using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using Control.Application.Features.Sensors.Commands.SyncSensorCreated;
using MediatR;

namespace Control.Infrastructure.Messaging.Sensor;

internal sealed class SensorCreatedEventconsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorCreatedEvent, SyncSensorCreatedCommand>(sender, mapper);

