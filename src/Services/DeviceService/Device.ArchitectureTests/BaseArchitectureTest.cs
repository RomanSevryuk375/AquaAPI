using System.Reflection;
using Device.API.Endpoints.Relays;
using Device.Application.Interfaces;
using Device.Domain.Entities;
using Device.Infrastructure.Persistence;

namespace Device.ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(Controller).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IDeviceSecurityService).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(DeviceDbContext).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(AddRelayEndpoint).Assembly;
}
