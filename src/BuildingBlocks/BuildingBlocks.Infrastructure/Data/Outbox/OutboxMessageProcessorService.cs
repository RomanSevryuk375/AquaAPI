using System.Data.Common;
using System.Text.Json;
using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Data.Outbox;

public sealed class OutboxMessageProcessorService<TDbContext>(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<OutboxMessageProcessorService<TDbContext>> logger)
    where TDbContext : DbContext
{
    public async Task<Result> ProcessAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        IEntityType? entityType = dbContext.Model.FindEntityType(typeof(OutboxMessage));
        string tableName = entityType?.GetTableName() ?? "outbox_messages";

        await using IDbContextTransaction transaction =
            await dbContext.Database.BeginTransactionAsync(cancellationToken);

        DbConnection connection = dbContext.Database.GetDbConnection();
        DbTransaction? dbTransaction = dbContext.Database.CurrentTransaction?.GetDbTransaction();

        string selectSql = $"""
            SELECT 
              id AS Id, 
              type AS Type, 
              content AS Content, 
              occurred_on_utc AS OccurredOnUtc
            FROM {tableName}
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc
            LIMIT 50
            FOR UPDATE SKIP LOCKED
            """;

#pragma warning disable S2077 // Dynamic SQL is safe here because tableName is retrieved from EF Core metadata
        var messages = (await connection.QueryAsync<OutboxMessage>(
            selectSql,
            transaction: dbTransaction)).ToList();
#pragma warning restore S2077 // Dynamic SQL is safe here because tableName is retrieved from EF Core metadata
        if (messages.Count == 0)
        {
            return Result.Success();
        }

        foreach (OutboxMessage message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                {
                    logger.LogWarning("Type {MessageType} not found for outbox message {MessageId}",
                        message.Type, message.Id);
                    message.Error = $"Type {message.Type} not found.";
                    message.ProcessedOnUtc = DateTimeOffset.UtcNow.UtcDateTime;
                    continue;
                }

                if (JsonSerializer.Deserialize(message.Content, type) is not IDomainEvent domainEvent)
                {
                    logger.LogWarning("Content of outbox message {MessageId} is not an IDomainEvent", message.Id);
                    message.Error = "Content is not an IDomainEvent.";
                    message.ProcessedOnUtc = DateTimeOffset.UtcNow.UtcDateTime;
                    continue;
                }

                await publisher.Publish(domainEvent, cancellationToken);
                message.ProcessedOnUtc = DateTimeOffset.UtcNow.UtcDateTime;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
                message.Error = ex.Message;
                message.ProcessedOnUtc = DateTimeOffset.UtcNow.UtcDateTime;
            }
        }

        string updateSql = $"""
            UPDATE {tableName}
            SET processed_on_utc = @ProcessedOnUtc, error = @Error
            WHERE id = @Id
            """;

#pragma warning disable S2077 // Dynamic SQL is safe here because tableName is retrieved from EF Core metadata
        await connection.ExecuteAsync(
            updateSql,
            messages.Select(m => new { m.ProcessedOnUtc, m.Error, m.Id }),
            transaction: dbTransaction);
#pragma warning restore S2077 // Dynamic SQL is safe here because tableName is retrieved from EF Core metadata

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
