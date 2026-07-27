using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Controllers.Command.PingController;

public sealed class PingControllerHandler(IControllerRepository controllerRepository)
    : IRequestHandler<PingControllerCommand, Result<ControllerPingResponse>>
{
    public async Task<Result<ControllerPingResponse>> Handle(
        PingControllerCommand request,
        CancellationToken cancellationToken)
    {
        Controller? controller = await controllerRepository.GetByIdAsync(
            request.ControllerId, cancellationToken);
        if (controller is null)
        {
            return Result<ControllerPingResponse>.Failure(Error.NotFound<Controller>(
                string.Format(ErrorMessages.ControllerNotFound, request.ControllerId)));
        }

        controller.RecordPing();

        return Result<ControllerPingResponse>.Success(new ControllerPingResponse());
    }
}
