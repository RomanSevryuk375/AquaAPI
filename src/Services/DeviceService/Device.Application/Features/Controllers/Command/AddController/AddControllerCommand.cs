using BuildingBlocks.Domain.Abstractions;

namespace Device.Application.Features.Controllers.Command.AddController;

public sealed record AddControllerCommand
    : ICommand<ControllerRegisteredResponse>, IUserBoundRequest
{
    public Guid UserId { get; init; }
    public string MacAddress { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public bool IsOnline { get; init; }
}
