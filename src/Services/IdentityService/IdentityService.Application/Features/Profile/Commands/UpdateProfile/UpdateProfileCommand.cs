using BuildingBlocks.Domain.Abstractions;

namespace IdentityService.Application.Features.Profile.Commands.UpdateProfile;

public sealed record UpdateProfileCommand : ICommand
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}
