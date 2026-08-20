using BuildingBlocks.Domain.Abstractions;

namespace Control.Application.Interfaces;

public interface IScheduleBoundRequest : IUserBoundRequest
{
    public Guid ScheduleId { get; }
}
