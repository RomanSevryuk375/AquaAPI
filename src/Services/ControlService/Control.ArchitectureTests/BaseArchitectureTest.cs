using System.Reflection;
using Control.API.Endpoints.AutomationRules;
using Control.Application.Interfaces;
using Control.Domain.Entities;
using Control.Infrastructure.Persistence;

namespace Control.ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(AutomationRule).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IRuleBoundRequest).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ControlDbContext).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(AddConditionEndpoint).Assembly;
}
