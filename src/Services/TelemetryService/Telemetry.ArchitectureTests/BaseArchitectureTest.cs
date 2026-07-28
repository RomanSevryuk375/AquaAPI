using System.Reflection;
using Telemetry.API.Endpoints;
using Telemetry.Application.Features.Telemetry.Commands.AddTelemetryBatch;
using Telemetry.Domain.Entities;
using Telemetry.Infrastructure.Persistence;

namespace Telemetry.ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(Ecosystem).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(AddTelemetryBatchCommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(TelemetryDbContext).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(GetRawDataEndpoint).Assembly;
}
