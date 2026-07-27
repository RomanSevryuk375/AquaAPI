using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Ecosystems;
using MediatR;
using Telemetry.Application.Features.Ecosystems.Commands.SyncEcosystemCreated;

namespace Telemetry.Infrastructure.Messaging.EcosystemConsumers;

internal sealed class EcosystemCreatedConsumer(ISender sender, IMapper mapper) :
    MediatRIntegrationEventConsumer<EcosystemCreatedEvent, SyncEcosystemCreatedCommand>(sender, mapper);

