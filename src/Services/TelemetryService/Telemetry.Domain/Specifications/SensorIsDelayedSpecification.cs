using Contracts.Enums;
using Telemetry.Domain.Entities;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Domain.Specifications;

public class SensorIsDelayedSpecification : BaseSpecification<SensorEntity>
{
    public SensorIsDelayedSpecification(DateTime offlineThreshold) : 
        base(data => 
            data.UpdatedAt < offlineThreshold && 
            data.State == SensorStateEnum.Active)
    {   
    }
}
