using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;
using Telemetry.Domain.SpecificationParams;

namespace Telemetry.Domain.Specifications;

public class TelemetryFilterSpecification : BaseSpecification<TelemetryDataEntity>
{
    public TelemetryFilterSpecification(TelemetryFilterParams @params)
        : base (data =>
            (!@params.SensorId.HasValue || data.SensorId == @params.SensorId) &&
            (!@params.From.HasValue || data.RecordedAt >= @params.From.Value) &&
            (!@params.To.HasValue || data.RecordedAt <= @params.To.Value))
    {
    }
}
