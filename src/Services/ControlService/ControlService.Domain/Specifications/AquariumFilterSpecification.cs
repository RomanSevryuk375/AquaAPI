using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Control.Domain.SpecificationParams;

namespace Control.Domain.Specifications;

public class AquariumFilterSpecification : BaseSpecification<AquariumEntity>
{
    public AquariumFilterSpecification(AquariumFilterParams @params) 
        : base(data => 
            (!@params.ControllerId.HasValue || data.ControllerId == @params.ControllerId.Value) &&
            (string.IsNullOrWhiteSpace(@params.Name)
                || data.Name.ToLower().Contains(@params.Name.ToLower())))
    {
    }
}
