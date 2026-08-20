using BuildingBlocks.Domain.Abstractions;

namespace Control.Application.Interfaces;

public interface IVacationModeBoundRequest : IUserBoundRequest
{
    public Guid VacationModeId { get; }
}
