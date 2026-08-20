using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.Ecosystems.Commands.UpdateEcosystem;

public sealed class UpdateEcosystemHandler(
    IEcosystemRepository ecosystemRepository,
    IFusionCache cache)
    : IRequestHandler<UpdateEcosystemCommand, Result>
{
    public async Task<Result> Handle(
        UpdateEcosystemCommand request,
        CancellationToken cancellationToken)
    {
        Ecosystem? ecosystem = await ecosystemRepository.GetByIdAsync(
            request.EcosystemId, cancellationToken);
        if (ecosystem is null)
        {
            return Result.Failure(Error.NotFound<Ecosystem>(
                $"Ecosystem {request.EcosystemId} not found"));
        }

        Result nameResult = ecosystem.SetName(request.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        Result volumeResult = ecosystem.SetVolume(request.Volume);
        if (volumeResult.IsFailure)
        {
            return Result.Failure(volumeResult.Error);
        }

        await cache.RemoveAsync(CacheKeys.Ecosystem(request.UserId, request.EcosystemId), token: cancellationToken);

        return Result.Success();
    }
}
