using System.Text.Json;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Infrastructure.Data.Outbox;
using Identity.Infrastructure.IntegrationTests.Infrastructure;
using IdentityService.Domain.Events;
using IdentityService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.IntegrationTests.BackgroundJobs;

public class OutboxMessageProcessorServiceTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "")]
    public async Task ProcessAsync_ShouldProcessValidMessageSuccessfully()
    {
        // Arrange
        var domainEvent = new UserUpdatedDomainEvent
        {
            UserId = Guid.NewGuid(),
            Name = "Success User",
            PhoneNumber = "+375291112233"
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(UserUpdatedDomainEvent).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(domainEvent)
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<IdentityDbContext> service = GetRequiredService<OutboxMessageProcessorService<IdentityDbContext>>();

        // Act
        Result result = await service.ProcessAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        OutboxMessage? processedMessage = await DbContext.Set<OutboxMessage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == outboxMessage.Id);

        processedMessage.Should().NotBeNull();
        processedMessage!.ProcessedOnUtc.Should().NotBeNull();
        processedMessage.Error.Should().BeNull();
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task ProcessAsync_ShouldMarkAsPoisonMessage_WhenTypeIsUnresolvable()
    {
        // Arrange
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = "NonExistentNamespace.NonExistentClass, NonExistentAssembly",
            Content = "{}"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<IdentityDbContext> service = GetRequiredService<OutboxMessageProcessorService<IdentityDbContext>>();

        // Act
        Result result = await service.ProcessAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        OutboxMessage? processedMessage = await DbContext.Set<OutboxMessage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == outboxMessage.Id);

        processedMessage.Should().NotBeNull();
        processedMessage!.ProcessedOnUtc.Should().NotBeNull();
        processedMessage.Error.Should().NotBeNull();

        processedMessage.Error.Should().Contain("not found");
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task ProcessAsync_ShouldMarkAsPoisonMessage_WhenContentIsInvalidJson()
    {
        // Arrange
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(UserUpdatedDomainEvent).AssemblyQualifiedName!,
            Content = "[]"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<IdentityDbContext> service = GetRequiredService<OutboxMessageProcessorService<IdentityDbContext>>();

        // Act
        Result result = await service.ProcessAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        OutboxMessage? processedMessage = await DbContext.Set<OutboxMessage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == outboxMessage.Id);

        processedMessage.Should().NotBeNull();
        processedMessage!.ProcessedOnUtc.Should().NotBeNull();
        processedMessage.Error.Should().NotBeNull();
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task ProcessAsync_ShouldMarkAsPoisonMessage_WhenContentDeserializesToNull()
    {
        // Arrange
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(UserUpdatedDomainEvent).AssemblyQualifiedName!,
            Content = "null"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<IdentityDbContext> service = GetRequiredService<OutboxMessageProcessorService<IdentityDbContext>>();

        // Act
        Result result = await service.ProcessAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        OutboxMessage? processedMessage = await DbContext.Set<OutboxMessage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == outboxMessage.Id);

        processedMessage.Should().NotBeNull();
        processedMessage!.ProcessedOnUtc.Should().NotBeNull();
        processedMessage.Error.Should().NotBeNull();

        processedMessage.Error.Should().Contain("Content is not an IDomainEvent");
    }
}
