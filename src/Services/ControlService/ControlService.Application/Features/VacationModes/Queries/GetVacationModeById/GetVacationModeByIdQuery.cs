using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Application.Features.VacationModes.Queries.Shared;
using Control.Application.Interfaces;

namespace Control.Application.Features.VacationModes.Queries.GetVacationModeById;

public sealed record GetVacationModeByIdQuery
    : IQuery<Result<VacationModeDto>>, IVacationModeBoundRequest
{
    public Guid VacationModeId { get; init; }
    public Guid UserId { get; init; }
}
