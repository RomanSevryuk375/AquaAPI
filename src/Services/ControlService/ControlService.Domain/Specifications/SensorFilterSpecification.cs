using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class SensorFilterSpecification : BaseSpecification<SensorEntity>
{
    public SensorFilterSpecification(SensorFilterParams @params) 
        : base(data => 
            (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.State.HasValue || data.State == @params.State.Value)
            && (!@params.Type.HasValue || data.Type == @params.Type.Value))
    {
        
    }
}
