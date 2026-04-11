using Contracts.Events.RelayEvents;
using Control.Application.Interfaces;
using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Application.Services;

public class RelayServiceFromEvent(
    IRelayRepository relayRepository,
    IAquariumRepository aquariumRepository,
    IUnitOfWork unitOfWork) : IRelayServiceFromEvent
{
    public async Task ChangedModeFromEventAsync(
        RelayModeChangedCommand relayMode, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayMode.RelayId, cancellationToken);

        if (existingRelay is null)
        {
            return;
        }

        existingRelay.SetMode(relayMode.IsManual);

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangedStateFromEventAsync(
        RelayStateChangedCommand relayState, 
        CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayState.RelayId, cancellationToken);

        if (existingRelay is null)
        {
            return;
        }

        existingRelay.SetState(relayState.IsActive);

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateRelayFromEventAsync(
        RelayCreatedEvent relayCreated, 
        CancellationToken cancellationToken)
    {
        if (await relayRepository.ExistsAsync(relayCreated.RelayId, cancellationToken))
        {
            return;
        }

        var existingAquarium = await aquariumRepository
            .GetByControllerIdAsync(relayCreated.ControllerId, cancellationToken);

        if (existingAquarium is null)
        {
            return;
        }

        var (relay, errors) = RelayEntity.Create(
            relayCreated.RelayId,
            existingAquarium.Id,
            relayCreated.Purpose,
            relayCreated.IsManual,
            relayCreated.IsActive,
            relayCreated.CreatedAt);

        if (errors.Count > 0)
        {
            return;
        }

        await relayRepository.AddAsync(relay!, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletedRelayFromEventAsync(RelayDeletedEvent relayDeleted, CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayDeleted.RelayId, cancellationToken);

        if (existingRelay is null)
        {
            return;
        }

        await relayRepository.DeleteAsync(relayDeleted.RelayId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatedRelayFromEventAsync(RelayUpdatedEvent relayUpdated, CancellationToken cancellationToken)
    {
        var existingRelay = await relayRepository
            .GetByIdAsync(relayUpdated.RelayId, cancellationToken);

        if (existingRelay is null)
        {
            var existingAquarium = await aquariumRepository
                .GetByControllerIdAsync(relayUpdated.ControllerId, cancellationToken);

            if (existingAquarium is null)
            {
                return;
            }

            var (relay, errors) = RelayEntity.Create(
            relayUpdated.RelayId,
            existingAquarium.Id,
            relayUpdated.Purpose,
            relayUpdated.IsManual,
            relayUpdated.IsActive,
            relayUpdated.CreatedAt);

            if (errors.Count > 0)
            {
                return;
            }

            await relayRepository.AddAsync(relay!, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return;
        }

        existingRelay!.SetPurpose(relayUpdated.Purpose);
        existingRelay.SetMode(relayUpdated.IsManual);
        existingRelay.SetState(relayUpdated.IsActive);

        await relayRepository.UpdateAsync(existingRelay, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
