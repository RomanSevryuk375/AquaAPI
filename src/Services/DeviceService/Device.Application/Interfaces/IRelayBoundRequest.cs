using BuildingBlocks.Domain.Abstractions;

namespace Device.Application.Interfaces;

public interface IRelayBoundRequest : IUserBoundRequest
{
    public Guid RelayId { get; }
}
