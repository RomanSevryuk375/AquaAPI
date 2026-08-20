using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;
using Device.Application.Constants;
using ZiggyCreatures.Caching.Fusion;

namespace Device.Application.Features.Controllers.Command.ToggleCommandState;

public sealed class ToggleControllerStateHandler(
    IControllerRepository controllerRepository,
    IFusionCache cache)
    : IRequestHandler<ToggleControllerStateCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        ToggleControllerStateCommand request,
        CancellationToken cancellationToken)
    {
        Controller? controller = await controllerRepository.GetByIdAsync(
            request.ControllerId, cancellationToken);
        if (controller is null)
        {
            return Result<bool>.Failure(Error.NotFound<Controller>(
                string.Format(ErrorMessages.ControllerNotFound, request.ControllerId)));
        }

        controller.ToggleState();

        await cache.RemoveAsync(CacheKeys.Controller(controller.UserId, controller.Id), token: cancellationToken);

        return Result<bool>.Success(controller.IsOnline);
    }
}
