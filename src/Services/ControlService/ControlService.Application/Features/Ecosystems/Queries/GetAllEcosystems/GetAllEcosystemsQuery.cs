using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Domain.Results;

namespace Control.Application.Features.Ecosystems.Queries.GetAllEcosystems;

public sealed record GetAllEcosystemsQuery
    : IQuery<Result<IReadOnlyList<EcosystemDto>>>
{
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public Guid? ControllerId { get; init; }
    public EcosystemType? Type { get; init; }
    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 10;
}
