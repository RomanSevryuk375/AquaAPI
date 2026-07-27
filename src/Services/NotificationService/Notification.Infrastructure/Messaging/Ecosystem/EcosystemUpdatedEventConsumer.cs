using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Ecosystems;
using MediatR;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemUpdated;

namespace Notification.Infrastructure.Messaging.Ecosystem;

internal sealed class EcosystemUpdatedEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<EcosystemUpdatedEvent, SyncEcosystemUpdatedCommand>(sender, mapper);
