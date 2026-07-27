using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using Control.Application.Features.Sensors.Commands.HandleSensorNoData;
using MediatR;

namespace Control.Infrastructure.Messaging.Sensor;

internal sealed class SensorNoDataEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorNoDataEvent, HandleSensorNoDataCommand>(sender, mapper);

