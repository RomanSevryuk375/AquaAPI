using BuildingBlocks.Domain.Abstractions;

namespace IdentityService.Application.Features.Auth.Commands.Logout;

public sealed record LogoutCommand : ICommand
{
    public Guid UserId { get; init; }
}
