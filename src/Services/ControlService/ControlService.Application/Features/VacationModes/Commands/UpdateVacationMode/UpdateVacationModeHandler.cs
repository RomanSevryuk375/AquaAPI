using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.VacationModes.Commands.UpdateVacationMode;

public sealed class UpdateVacationModeHandler(
    IVacationModeRepository vacationModeRepository,
    IFusionCache cache)
    : IRequestHandler<UpdateVacationModeCommand, Result>
{
    public async Task<Result> Handle(UpdateVacationModeCommand request, CancellationToken cancellationToken)
    {
        VacationMode? vacationMode = await vacationModeRepository.GetByIdAsync(
            request.VacationModeId, cancellationToken);

        Result timingResult = vacationMode!.SetTiming(request.StartDate, request.EndDate);
        if (timingResult.IsFailure)
        {
            return Result.Failure(timingResult.Error);
        }

        Result feedResult = vacationMode.SetFeedSize(request.CalculatedFeed);
        if (feedResult.IsFailure)
        {
            return Result.Failure(feedResult.Error);
        }

        await cache.RemoveAsync(CacheKeys.VacationMode(request.UserId, request.VacationModeId), token: cancellationToken);

        return Result.Success();
    }
}
