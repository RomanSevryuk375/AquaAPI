using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class ScheduleFilterSpecification : BaseSpecification<ScheduleEntity>
{
    public ScheduleFilterSpecification(ScheduleFilterParams @params) 
        : base(data =>
            (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.RelayId.HasValue || data.RelayId == @params.RelayId.Value)
            && (!@params.IsFadeMode.HasValue || data.IsFadeMode == @params.IsFadeMode.Value)
            && (!@params.IsEnable.HasValue || data.IsEnable == @params.IsEnable.Value)) 
    {
        
    }
}
