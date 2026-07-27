using System.Text.Json;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Infrastructure.Data.Outbox;
using Control.Domain.Events;

namespace Control.Infrastructure.IntegrationTests.BackgroundJobs;

public class OutboxMessageProcessorServiceTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "")]
    public async Task ProcessAsync_ShouldProcessValidMessageSuccessfully()
    {
        // Arrange
        var domainEvent = new EcosystemCreatedDomainEvent
        {
            EcosystemId = Guid.NewGuid(),
            Name = "Success Ecosystem",
            UserId = UserContext.UserId,
            ControllerId = Guid.NewGuid()
        };

        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = typeof(EcosystemCreatedDomainEvent).AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(domainEvent)
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<ControlDbContext> service = GetRequiredService<OutboxMessageProcessorService<ControlDbContext>>();

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

        OutboxMessageProcessorService<ControlDbContext> service = GetRequiredService<OutboxMessageProcessorService<ControlDbContext>>();

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
            Type = typeof(EcosystemCreatedDomainEvent).AssemblyQualifiedName!,
            Content = "[]"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<ControlDbContext> service = GetRequiredService<OutboxMessageProcessorService<ControlDbContext>>();

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
            Type = typeof(EcosystemCreatedDomainEvent).AssemblyQualifiedName!,
            Content = "null"
        };

        DbContext.Set<OutboxMessage>().Add(outboxMessage);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        OutboxMessageProcessorService<ControlDbContext> service = GetRequiredService<OutboxMessageProcessorService<ControlDbContext>>();

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
