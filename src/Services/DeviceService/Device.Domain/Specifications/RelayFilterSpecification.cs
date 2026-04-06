using Contracts.Enums;
using Device.Domain.Entities;
using Device.Domain.Interfaces;

namespace Device.Domain.Specifications;

public class RelayFilterSpecification : BaseSpecification<RelayEntity>
{
    public RelayFilterSpecification(
        Guid? controllerId, 
        RelayPurposeEnum? purpose,
        bool? isActive,
        bool? isManual) : 
            base(data => 
                (!controllerId.HasValue || data.ControllerId == controllerId.Value) &&
                (!purpose.HasValue || data.Purpose == purpose.Value) &&
                (!isActive.HasValue || data.IsActive == isActive.Value) &&
                (!isManual.HasValue || data.IsManual == isManual.Value))
    {
    }
}

