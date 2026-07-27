using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Controllers.Command.ToggleCommandState;

public sealed class ToggleControllerStateHandler(IControllerRepository controllerRepository)
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

        return Result<bool>.Success(controller.IsOnline);
    }
}
