using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Sensors;
using Device.Application.Features.Sensors.Command.SetSensorState;
using MediatR;

namespace Device.Infrastructure.Messaging;

internal sealed class SensorNoDataConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<SensorNoDataEvent, SetSensorStateCommand>(sender, mapper);
