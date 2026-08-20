using BuildingBlocks.Domain.Abstractions;

namespace Control.Application.Interfaces;

public interface IEcosystemBoundRequest : IUserBoundRequest
{
    public Guid EcosystemId { get; }
}
