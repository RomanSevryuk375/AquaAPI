using BuildingBlocks.Domain.Results;
using IdentityService.Domain.Interfaces;
using MediatR;

namespace IdentityService.Application.Features.Auth.Commands.Logout;

public sealed class LogoutHandler(
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await refreshTokenRepository.DeleteTokensByUserIdAsync(request.UserId, cancellationToken);

        return Result.Success();
    }
}
