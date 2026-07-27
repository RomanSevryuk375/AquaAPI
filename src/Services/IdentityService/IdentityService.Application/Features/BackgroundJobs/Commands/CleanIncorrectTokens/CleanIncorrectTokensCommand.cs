using BuildingBlocks.Domain.Abstractions;

namespace IdentityService.Application.Features.BackgroundJobs.Commands.CleanIncorrectTokens;

public sealed record CleanIncorrectTokensCommand : ICommand
{
}
