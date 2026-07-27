using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Ecosystems;
using MediatR;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemCreated;

namespace Notification.Infrastructure.Messaging.Ecosystem;

internal sealed class EcosystemCreatedEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<EcosystemCreatedEvent, SyncEcosystemCreatedCommand>(sender, mapper);

