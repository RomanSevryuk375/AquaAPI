using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.VacationModes.Commands.DeleteVacationMode;

public sealed class DeleteVacationModeHandler(
    IVacationModeRepository vacationModeRepository,
    IFusionCache cache)
    : IRequestHandler<DeleteVacationModeCommand, Result>
{
    public async Task<Result> Handle(DeleteVacationModeCommand request, CancellationToken cancellationToken)
    {
        await vacationModeRepository.DeleteAsync(request.VacationModeId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.VacationMode(request.UserId, request.VacationModeId), token: cancellationToken);

        return Result.Success();
    }
}
