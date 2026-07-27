using BuildingBlocks.Domain.Constants;
using BuildingBlocks.Domain.Results;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Notification.Application.Features.BackgroundJobs.Commands.ProcessUnpublishedNotices;
using Notification.Application.InternalEvents;
using Notification.Domain.Entities;
using Notification.Infrastructure.IntegrationTests.Infrastructure;
using Notification.TestShared.Builders;
using DomainNotification = Notification.Domain.Entities.Notification;

namespace Notification.Infrastructure.IntegrationTests.Features.BackgroundJobs;

public class ProcessUnpublishedNoticesHandlerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_ShouldMarkNotificationAsPublished_AndPublishCommands_WhenUserHasChannelsEnabled()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        User user = new UserBuilder()
            .WithId(userId)
            .WithEmail("user@example.com")
            .WithEnable(true)
            .WithEmailEnable(true)
            .Build();

        DomainNotification notification = new NotificationBuilder()
            .WithId(notificationId)
            .WithUserId(userId)
            .WithEcosystemId(null)
            .WithMessage("Success notification message")
            .WithIsPublished(false)
            .Build();

        DbContext.Set<User>().Add(user);
        DbContext.Set<DomainNotification>().Add(notification);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        ITestHarness harness = GetRequiredService<ITestHarness>();
        var command = new ProcessUnpublishedNoticesCommand();

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        DomainNotification? updatedNotification = await DbContext.Set<DomainNotification>()
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == notificationId);

        updatedNotification.Should().NotBeNull();
        updatedNotification!.IsPublished.Should().BeTrue();
        updatedNotification.FailureReason.Should().BeNull();
        updatedNotification.RetryCount.Should().Be(0);

        bool published = await harness.Published.Any<SendEmailCommand>(x =>
            x.Context.Message.NotificationId == notificationId &&
            x.Context.Message.Email == "user@example.com" &&
            x.Context.Message.Message == "Success notification message");

        published.Should().BeTrue();
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task Handle_ShouldIncrementRetryCountAndSetFailureReason_WhenUserDisabledOrNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        User user = new UserBuilder()
            .WithId(userId)
            .WithEmail("user@example.com")
            .WithEnable(false)
            .Build();

        DomainNotification notification = new NotificationBuilder()
            .WithId(notificationId)
            .WithUserId(userId)
            .WithEcosystemId(null)
            .WithMessage("Failure notification message")
            .WithIsPublished(false)
            .Build();

        DbContext.Set<User>().Add(user);
        DbContext.Set<DomainNotification>().Add(notification);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        ITestHarness harness = GetRequiredService<ITestHarness>();
        var command = new ProcessUnpublishedNoticesCommand();

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        DomainNotification? updatedNotification = await DbContext.Set<DomainNotification>()
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == notificationId);

        updatedNotification.Should().NotBeNull();
        updatedNotification!.IsPublished.Should().BeFalse();
        updatedNotification.RetryCount.Should().Be(1);
        updatedNotification.FailureReason.Should().Be(ErrorMessages.NotificationProvider.UserDisabledOrNotFound);

        bool published = await harness.Published.Any<SendEmailCommand>(x => x.Context.Message.NotificationId == notificationId);
        published.Should().BeFalse();
    }
}
