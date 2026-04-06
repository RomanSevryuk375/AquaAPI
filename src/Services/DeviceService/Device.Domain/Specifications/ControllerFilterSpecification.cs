using Device.Domain.Entities;
using Device.Domain.Interfaces;

namespace Device.Domain.Specifications;

public class ControllerFilterSpecification : BaseSpecification<ControllerEntity>
{
    public ControllerFilterSpecification(string? searchTerm, bool? isOnline) 
        : base(data => 
            (!isOnline.HasValue || data.IsOnline == isOnline.Value) &&
            (string.IsNullOrWhiteSpace(searchTerm) 
                || data.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) 
                || data.MacAddress.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)))
    {
    }
}
