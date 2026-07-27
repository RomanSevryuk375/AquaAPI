using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using MediatR;
using Telemetry.Application.Features.Sensors.Commands.SyncSensorDeleted;

namespace Telemetry.Infrastructure.Messaging.SensorConsumers;

internal sealed class SensorDeletedConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorDeletedEvent, SyncSensorDeletedCommand>(sender, mapper);
