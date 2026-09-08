using BuildingBlocks.Domain.Results;
using BuildingBlocks.Infrastructure.Data.Outbox;
using IdentityService.Application.Features.Profile.Commands.UpdateProfile;

namespace Identity.Infrastructure.IntegrationTests.Features.Profile.Commands.UpdateProfile;

public class UpdateProfileHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_ShouldUpdateProfileAndCreateOutboxMessage_WhenCommandIsValid()
    {
        User user = await CreateUserWithSubscriptionAsync("Old Name", "alice@aquasmart.com", "+375295554433");

        var command = new UpdateProfileCommand
        {
            UserId = UserContext.UserId,
            Name = "New Name",
            PhoneNumber = "+375296667788"
        };

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        User? updatedUser = await DbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        updatedUser.Should().NotBeNull();
        updatedUser!.Name.Value.Should().Be("New Name");
        updatedUser.PhoneNumber.Should().Be("+375296667788");

        List<OutboxMessage> outboxMessages = await DbContext.OutboxMessages
            .AsNoTracking()
            .ToListAsync();

        outboxMessages.Should().ContainSingle(m => m.Type.Contains("UserUpdatedDomainEvent"));
        OutboxMessage outboxMessage = outboxMessages.Single(m => m.Type.Contains("UserUpdatedDomainEvent"));
        outboxMessage.Content.Should().Contain("New Name");
        outboxMessage.Content.Should().Contain("+375296667788");
    }
}
