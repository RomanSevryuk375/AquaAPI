using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Ecosystems;
using MediatR;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemDeleted;

namespace Notification.Infrastructure.Messaging.Ecosystem;

internal sealed class EcosystemDeletedEventConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<EcosystemDeletedEvent, SyncEcosystemDeletedCommand>(sender, mapper);
