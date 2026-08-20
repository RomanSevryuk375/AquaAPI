using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Application.Interfaces;

namespace Control.Application.Features.Ecosystems.Queries.GetEcosystemById;

public sealed record GetEcosystemByIdQuery
    : ICachedQuery<Result<EcosystemDto>>, IEcosystemBoundRequest
{
    public Guid EcosystemId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.Ecosystem(UserId, EcosystemId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
