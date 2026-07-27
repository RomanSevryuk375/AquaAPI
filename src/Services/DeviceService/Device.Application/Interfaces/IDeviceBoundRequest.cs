namespace Device.Application.Interfaces;

public interface IDeviceBoundRequest : IControllerBoundRequest
{
    public string DeviceToken { get; }
}
