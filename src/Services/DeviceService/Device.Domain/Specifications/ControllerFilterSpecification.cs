using Device.Domain.Entities;
using Device.Domain.Interfaces;
using Device.Domain.SpecificationParams;

namespace Device.Domain.Specifications;

public class ControllerFilterSpecification : BaseSpecification<ControllerEntity>
{
    public ControllerFilterSpecification(ControllerFilterParams @params) 
        : base(data => 
            (!@params.IsOnline.HasValue || data.IsOnline == @params.IsOnline.Value) &&
            (string.IsNullOrWhiteSpace(@params.SearchTerm)
                || data.Name.ToLower().Contains(@params.SearchTerm.ToLower())
                || data.MacAddress.ToLower().Contains(@params.SearchTerm.ToLower())))
    {
    }
}
