using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Ecosystems;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemCreated;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemDeleted;
using Notification.Application.Features.Ecosystems.Commands.SyncEcosystemUpdated;

namespace Notification.Application.MapProfiles;

public sealed class EcosystemProfile : Profile
{
    public EcosystemProfile()
    {
        CreateMap<SyncEcosystemUpdatedCommand, SyncEcosystemCreatedCommand>();

        CreateMap<EcosystemCreatedEvent, SyncEcosystemCreatedCommand>();

        CreateMap<EcosystemUpdatedEvent, SyncEcosystemUpdatedCommand>();

        CreateMap<EcosystemDeletedEvent, SyncEcosystemDeletedCommand>();
    }
}

