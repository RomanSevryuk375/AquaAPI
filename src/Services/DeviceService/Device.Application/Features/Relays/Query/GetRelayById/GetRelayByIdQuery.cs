using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Device.Application.Constants;
using Device.Application.Features.Relays.Query.Shared;

namespace Device.Application.Features.Relays.Query.GetRelayById;

public sealed record GetRelayByIdQuery
    : ICachedQuery<Result<RelayDto>>
{
    public Guid UserId { get; init; }
    public Guid RelayId { get; init; }

    public string CacheKey => CacheKeys.Relay(UserId, RelayId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
