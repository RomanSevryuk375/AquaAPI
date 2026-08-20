using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Device.Application.Constants;
using Device.Application.Features.Controllers.Query.Shared;

namespace Device.Application.Features.Controllers.Query.GetControllerById;

public sealed record GetControllerByIdQuery
    : ICachedQuery<Result<ControllerDto>>
{
    public Guid UserId { get; init; }
    public Guid ControllerId { get; init; }

    public string CacheKey => CacheKeys.Controller(UserId, ControllerId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}
