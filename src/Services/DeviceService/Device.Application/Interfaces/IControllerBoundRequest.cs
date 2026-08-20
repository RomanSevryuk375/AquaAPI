using BuildingBlocks.Domain.Abstractions;

namespace Device.Application.Interfaces;

public interface IControllerBoundRequest : IUserBoundRequest
{
    public Guid ControllerId { get; }
}
