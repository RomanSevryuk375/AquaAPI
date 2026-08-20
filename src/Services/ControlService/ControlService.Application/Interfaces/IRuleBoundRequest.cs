using BuildingBlocks.Domain.Abstractions;

namespace Control.Application.Interfaces;

public interface IRuleBoundRequest : IUserBoundRequest
{
    public Guid RuleId { get; }
}
