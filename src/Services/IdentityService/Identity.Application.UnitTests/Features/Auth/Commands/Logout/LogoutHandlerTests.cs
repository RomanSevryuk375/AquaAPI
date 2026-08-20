using BuildingBlocks.Domain.Results;
using IdentityService.Application.Features.Auth.Commands.Logout;

namespace Identity.Application.UnitTests.Features.Auth.Commands.Logout;

public class LogoutHandlerTests
{
    private readonly IRefreshTokenRepository _tokenRepoMock = Substitute.For<IRefreshTokenRepository>();
    private readonly LogoutHandler _handler;

    public LogoutHandlerTests()
    {
        _handler = new LogoutHandler(_tokenRepoMock);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_WithValidUserId_DeletesTokensByUserIdAndReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new LogoutCommand { UserId = userId };

        // Act
        Result result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _tokenRepoMock.Received(1).DeleteTokensByUserIdAsync(userId, Arg.Any<CancellationToken>());
    }
}
