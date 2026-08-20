using BuildingBlocks.Domain.Abstractions;
using Control.Application.Interfaces;

namespace Control.Application.Features.VacationModes.Commands.ToggleVacationMode;

public sealed record ToggleVacationModeCommand
    : ICommand, IVacationModeBoundRequest
{
    public Guid UserId { get; init; }
    public Guid VacationModeId { get; init; }
}
