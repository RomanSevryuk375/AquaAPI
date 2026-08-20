using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Application.Features.VacationModes.Queries.Shared;
using Control.Application.Interfaces;

namespace Control.Application.Features.VacationModes.Queries.GetVacationModeById;

public sealed record GetVacationModeByIdQuery
    : ICachedQuery<Result<VacationModeDto>>, IVacationModeBoundRequest
{
    public Guid VacationModeId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.VacationMode(UserId, VacationModeId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
