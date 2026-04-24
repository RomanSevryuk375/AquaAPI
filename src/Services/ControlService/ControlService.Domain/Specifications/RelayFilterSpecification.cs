using Contracts.Abstractions;
using Control.Domain.Entities;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class RelayFilterSpecification : BaseSpecification<RelayEntity>
{
    public RelayFilterSpecification(RelayFilterParams @params) 
        : base(data => 
            (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.Purpose.HasValue || data.Purpose == @params.Purpose.Value)
            && (!@params.IsManual.HasValue || data.IsManual == @params.IsManual.Value)
            && (!@params.IsActive.HasValue || data.IsActive == @params.IsActive.Value))
    {
        
    }
}
