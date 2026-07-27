using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Relays;
using Control.Application.Features.Relays.Commands.SyncRelayDeleted;
using MediatR;

namespace Control.Infrastructure.Messaging.Relay;

internal sealed class RelayDeletedEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<RelayDeletedEvent, SyncRelayDeletedCommand>(sender, mapper);
