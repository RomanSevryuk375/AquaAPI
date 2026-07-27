using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.Relays.Command.DeleteRelay;

public sealed class DeleteRelayHandler(IRelayRepository relayRepository)
    : IRequestHandler<DeleteRelayCommand, Result>
{
    public async Task<Result> Handle(
        DeleteRelayCommand request,
        CancellationToken cancellationToken)
    {
        Relay? existingRelay = await relayRepository.GetByIdAsync(
            request.RelayId, cancellationToken);

        existingRelay!.MarkAsDeleted();

        await relayRepository.DeleteAsync(request.RelayId, cancellationToken);

        return Result.Success();
    }
}
