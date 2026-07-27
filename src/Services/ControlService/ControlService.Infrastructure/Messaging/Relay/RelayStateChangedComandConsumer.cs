using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Relays;
using Control.Application.Features.Relays.Commands.SyncRelayState;
using MediatR;

namespace Control.Infrastructure.Messaging.Relay;

internal sealed class RelayStateChangedComandConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<ChangeRelayStateEvent, SyncRelayStateCommand>(sender, mapper);
