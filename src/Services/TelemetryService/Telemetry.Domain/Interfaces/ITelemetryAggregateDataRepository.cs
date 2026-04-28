using Contracts.Abstractions;
using Telemetry.Domain.Entities;

namespace Telemetry.Domain.Interfaces;

public interface ITelemetryAggregateDataRepository : IRepository<TelemetryAggregateEntity>
{
}