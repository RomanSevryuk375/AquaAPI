using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Controllers.Command.UpdateController;

internal sealed class UpdateControllerHandler(IControllerRepository controllerRepository)
    : IRequestHandler<UpdateControllerCommand, Result>
{
    public async Task<Result> Handle(
        UpdateControllerCommand request,
        CancellationToken cancellationToken)
    {
        Controller? controller = await controllerRepository.GetByIdAsync(
            request.ControllerId, cancellationToken);
        if (controller is null)
        {
            return Result<bool>.Failure(Error.NotFound<Controller>(
                string.Format(ErrorMessages.ControllerNotFound, request.ControllerId)));
        }

        Result? result = controller.Update(request.MacAddress, request.Name);

        return result.IsFailure
            ? Result.Failure(result.Error)
            : Result.Success();
    }
}
