using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Domain.Specifications;

public class TelemetryFilterSpecification : BaseSpecification<TelemetryDataEntity>
{
    public TelemetryFilterSpecification(Guid? sensorId, DateTime? from, DateTime? to)
        : base (data =>
            (!sensorId.HasValue || data.SensorId == sensorId) &&
            (!from.HasValue || data.RecordedAt >= from.Value) &&
            (!to.HasValue || data.RecordedAt <= to.Value))
    {
    }
}
