using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Device.Application.Features.Relays.Query.Shared;

namespace Device.Application.Features.Relays.Query.GetRelayById;

public sealed record GetRelayByIdQuery
    : IQuery<Result<RelayDto>>
{
    public Guid UserId { get; init; }
    public Guid RelayId { get; init; }
}
