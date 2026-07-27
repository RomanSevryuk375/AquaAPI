using BuildingBlocks.Domain.Abstractions;

namespace Device.Application.Features.RelayCommands.Command.DeleteCompleted;

public sealed record DeleteCompletedCommand : ICommand<int>;
