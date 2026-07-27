using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Controllers.Command.DeleteController;

internal sealed class DeleteControllerHandler(IControllerRepository controllerRepository)
    : IRequestHandler<DeleteControllerCommand, Result>
{
    public async Task<Result> Handle(
        DeleteControllerCommand request,
        CancellationToken cancellationToken)
    {
        await controllerRepository.DeleteAsync(request.ControllerId, cancellationToken);

        return Result.Success();
    }
}
