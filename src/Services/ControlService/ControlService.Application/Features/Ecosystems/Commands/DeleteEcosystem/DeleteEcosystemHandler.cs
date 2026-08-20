using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.Ecosystems.Commands.DeleteEcosystem;

public sealed class DeleteEcosystemHandler(
    IEcosystemRepository ecosystemRepository,
    IFusionCache cache)
    : IRequestHandler<DeleteEcosystemCommand, Result>
{
    public async Task<Result> Handle(
        DeleteEcosystemCommand request,
        CancellationToken cancellationToken)
    {
        Ecosystem? ecosystem = await ecosystemRepository.GetByIdAsync(
            request.EcosystemId, cancellationToken);
        if (ecosystem is null)
        {
            return Result.Failure(Error.NotFound<Ecosystem>(
                $"Ecosystem {request.EcosystemId} not found"));
        }

        ecosystem.MarkAsDeleted();

        await ecosystemRepository.DeleteAsync(request.EcosystemId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.Ecosystem(request.UserId, request.EcosystemId), token: cancellationToken);

        return Result.Success();
    }
}
