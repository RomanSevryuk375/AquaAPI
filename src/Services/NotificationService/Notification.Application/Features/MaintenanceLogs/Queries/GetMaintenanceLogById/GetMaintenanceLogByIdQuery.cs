using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Notification.Application.Constants;
using Notification.Application.Features.MaintenanceLogs.Queries.Shared;

namespace Notification.Application.Features.MaintenanceLogs.Queries.GetMaintenanceLogById;

public sealed record GetMaintenanceLogByIdQuery
    : ICachedQuery<Result<MaintenanceLogDto>>
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.MaintenanceLog(UserId, Id);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
