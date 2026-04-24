using Contracts.Abstractions;
using Control.Domain.Entities;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class VacationModeFilterSpecification : BaseSpecification<VacationModeEntity>
{
    public VacationModeFilterSpecification(VacationModeFilterParams @params) 
        : base(data => 
            (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.StartDate.HasValue || data.StartDate > @params.StartDate.Value)
            && (!@params.EndDate.HasValue || data.EndDate < @params.EndDate.Value)
            && (!@params.IsActive.HasValue || data.IsActive == @params.IsActive.Value))
    {
        
    }
}
