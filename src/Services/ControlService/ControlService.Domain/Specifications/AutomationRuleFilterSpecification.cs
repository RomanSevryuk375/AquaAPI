using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class AutomationRuleFilterSpecification 
    : BaseSpecification<AutomationRuleEntity>
{
    public AutomationRuleFilterSpecification(AutomatizationRuleFilterParams @params) 
        : base(data => 
            (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.SensorId.HasValue || data.SensorId == @params.SensorId.Value)
            && (!@params.RelayId.HasValue || data.RelayId == @params.RelayId.Value)
            && (!@params.Condition.HasValue || data.Condition == @params.Condition.Value)
            && (!@params.Action.HasValue || data.Action == @params.Action.Value))
    {
        
    }
}
