using BuildingBlocks.Domain.Abstractions;

namespace Notification.Application.Interfaces;

public interface IEcosystemBoundRequest : IUserBoundRequest
{
    public Guid EcosystemId { get; }
}
