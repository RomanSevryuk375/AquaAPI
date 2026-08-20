using BuildingBlocks.Domain.Abstractions;
using Control.Application.Interfaces;

namespace Control.Application.Features.Schedules.Commands.SetIsActiveSchedule;

public sealed record SetIsActiveScheduleCommand
    : ICommand, IScheduleBoundRequest
{
    public Guid UserId { get; init; }
    public Guid ScheduleId { get; init; }
    public bool IsActive { get; init; }
}
