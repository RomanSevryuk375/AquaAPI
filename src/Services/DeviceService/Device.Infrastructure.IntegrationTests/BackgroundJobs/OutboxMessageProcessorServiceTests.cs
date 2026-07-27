using System.Text.Json;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Infrastructure.Data.Outbox;
using Device.Domain.Events.ControllerEvents;

namespace Device.Infrastructure.IntegrationTests.BackgroundJobs;

public class OutboxMessageProcessorServiceTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public async Task ProcessAsync_ShouldProcessValidMessageSuccessfully()
    {
        // Arrange
        var domainEvent = new ControllerNotOnlineDomainEvent
        {
            OccurredOn = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            ControllerId = Guid.NewGuid(),
            LastSeenAt = DateTime.UtcNow
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(ControllerNotOnlineDomainEvent).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(domainEvent)
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<DeviceDbContext> service = GetRequiredService<OutboxMessageProcessorService<DeviceDbContext>>();

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

        OutboxMessageProcessorService<DeviceDbContext> service = GetRequiredService<OutboxMessageProcessorService<DeviceDbContext>>();

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
            Type = typeof(ControllerNotOnlineDomainEvent).AssemblyQualifiedName!,
            Content = "[]"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<DeviceDbContext> service = GetRequiredService<OutboxMessageProcessorService<DeviceDbContext>>();

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
            Type = typeof(ControllerNotOnlineDomainEvent).AssemblyQualifiedName!,
            Content = "null"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<DeviceDbContext> service = GetRequiredService<OutboxMessageProcessorService<DeviceDbContext>>();

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
