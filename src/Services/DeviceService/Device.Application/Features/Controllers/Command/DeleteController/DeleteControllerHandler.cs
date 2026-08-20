using BuildingBlocks.Domain.Results;
using Device.Application.Constants;
using ZiggyCreatures.Caching.Fusion;

namespace Device.Application.Features.Controllers.Command.DeleteController;

internal sealed class DeleteControllerHandler(
    IControllerRepository controllerRepository,
    IFusionCache cache)
    : IRequestHandler<DeleteControllerCommand, Result>
{
    public async Task<Result> Handle(
        DeleteControllerCommand request,
        CancellationToken cancellationToken)
    {
        await controllerRepository.DeleteAsync(request.ControllerId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.Controller(request.UserId, request.ControllerId), token: cancellationToken);

        return Result.Success();
    }
}
