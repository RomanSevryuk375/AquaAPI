using BuildingBlocks.Domain.Abstractions;

namespace IdentityService.Application.Features.Profile.Commands.ChangePassword;

public sealed record ChangePasswordCommand : ICommand
{
    public Guid UserId { get; init; }
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
