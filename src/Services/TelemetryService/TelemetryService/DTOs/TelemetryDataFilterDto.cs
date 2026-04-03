using System.Linq.Expressions;
using Telemetry.Domain.Entities;

namespace Telemetry.API.DTOs;

public record TelemetryDataFilterDto
{
    public Guid? SensorId { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }

    public Expression<Func<TelemetryDataEntity, bool>> ToExpression()
    {
        return data =>
            (SensorId.HasValue || data.SensorId == SensorId) &&
            (From.HasValue || data.RecordedAt >= From.Value) &&
            (To.HasValue || data.RecordedAt <= To.Value);
    }
}
