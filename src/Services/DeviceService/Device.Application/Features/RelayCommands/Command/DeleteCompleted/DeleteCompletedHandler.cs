using BuildingBlocks.Domain.Results;

namespace Device.Application.Features.RelayCommands.Command.DeleteCompleted;

public sealed class DeleteCompletedHandler(IRelayCommandsRepository commandsRepository)
    : IRequestHandler<DeleteCompletedCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        DeleteCompletedCommand request,
        CancellationToken cancellationToken)
    {
        int deletedCount = await commandsRepository.DeleteCompletedAsync(cancellationToken);

        return Result<int>.Success(deletedCount);
    }
}
