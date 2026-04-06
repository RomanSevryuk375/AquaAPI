using Contracts.Enums;
using Device.Domain.Entities;
using Device.Domain.Interfaces;

namespace Device.Domain.Specifications;

public class SensorFilterSpecification : BaseSpecification<SensorEntity>
{
    public SensorFilterSpecification(Guid? controllerId, SensorTypeEnum? type) 
        : base(data => 
            (!controllerId.HasValue || data.ControllerId == controllerId.Value) &&
            (!type.HasValue || data.Type == type.Value))
    {
    }
}
