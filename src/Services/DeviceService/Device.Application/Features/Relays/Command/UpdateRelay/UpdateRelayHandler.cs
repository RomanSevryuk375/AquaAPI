using BuildingBlocks.Domain.Results;
using Device.Application.Interfaces;

namespace Device.Application.Features.Relays.Command.UpdateRelay;

internal sealed class UpdateRelayHandler(
    IRelayRepository relayRepository,
    IDeviceSecurityService securityService) : IRequestHandler<UpdateRelayCommand, Result>
{
    public async Task<Result> Handle(
        UpdateRelayCommand request,
        CancellationToken cancellationToken)
    {
        Relay? existingRelay = await relayRepository.GetByIdAsync(
            request.RelayId, cancellationToken);

        if (request.ControllerId != existingRelay!.ControllerId)
        {
            Result newControllerOwnership = await securityService.EnsureUserOwnsControllerAsync(
                request.ControllerId, request.UserId, cancellationToken);
            if (newControllerOwnership.IsFailure)
            {
                return newControllerOwnership;
            }
        }

        Result result = existingRelay.Update(
            request.ControllerId, request.UserId,
            request.ConnectionProtocol, request.ConnectionAddress,
            request.Purpose, request.IsNormallyOpen);

        return result.IsFailure
            ? Result.Failure(result.Error)
            : Result.Success();
    }
}
