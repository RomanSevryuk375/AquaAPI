using BuildingBlocks.Domain.Abstractions;

namespace Device.Application.Interfaces;

public interface ISensorBoundRequest : IUserBoundRequest
{
    public Guid SensorId { get; }
}
