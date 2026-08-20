using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.VacationModes.Commands.ToggleVacationMode;

public sealed class ToggleVacationModeHandler(
    IVacationModeRepository vacationModeRepository,
    IFusionCache cache)
    : IRequestHandler<ToggleVacationModeCommand, Result>
{
    public async Task<Result> Handle(ToggleVacationModeCommand request, CancellationToken cancellationToken)
    {
        VacationMode? vacationMode = await vacationModeRepository.GetByIdAsync(
            request.VacationModeId, cancellationToken);

        vacationMode!.SetActive(!vacationMode.IsActive);

        await cache.RemoveAsync(CacheKeys.VacationMode(request.UserId, request.VacationModeId), token: cancellationToken);

        return Result.Success();
    }
}
