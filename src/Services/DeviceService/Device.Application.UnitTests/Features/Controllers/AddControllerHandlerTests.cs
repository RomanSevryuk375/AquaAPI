using BuildingBlocks.Domain.Results;
using Device.Application.Features.Controllers.Command.AddController;

namespace Device.Application.UnitTests.Features.Controllers;

public class AddControllerHandlerTests
{
    private readonly IMyHasher _hasherMock = Substitute.For<IMyHasher>();
    private readonly IControllerRepository _controllerRepoMock = Substitute.For<IControllerRepository>();
    private readonly AddControllerHandler _handler;

    public AddControllerHandlerTests()
    {
        _handler = new AddControllerHandler(_hasherMock, _controllerRepoMock);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_WithValidCommand_GeneratesRawTokenHashesItAndSaves()
    {
        // Arrange
        var userId = Guid.NewGuid();

        string expectedHash = "some_generated_hash";

        _hasherMock.Generate(Arg.Any<string>()).Returns(expectedHash);

        var command = new AddControllerCommand
        {
            UserId = userId,
            MacAddress = TestConstants.ValidMacAddress,
            Name = "New Controller",
            IsOnline = true
        };

        // Act
        Result<ControllerRegisteredResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DeviceToken.Should().NotBeNullOrWhiteSpace();
        result.Value.ControllerId.Should().NotBeEmpty();

        await _controllerRepoMock.Received(1).AddAsync(
            Arg.Is<Controller>(c => c.DeviceTokenHash == expectedHash &&
                                    c.MacAddress.Value == TestConstants.ValidMacAddress),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_WithDomainValidationError_ReturnsFailureAndDoesNotSave()
    {
        var userId = Guid.NewGuid();
        // Arrange
        var command = new AddControllerCommand
        {

            UserId = userId
,
            MacAddress = "invalid_mac",
            Name = "New Controller",
            IsOnline = true
        };

        // Act
        Result<ControllerRegisteredResponse> result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        await _controllerRepoMock.DidNotReceiveWithAnyArgs().AddAsync(default!, default);
    }
}
