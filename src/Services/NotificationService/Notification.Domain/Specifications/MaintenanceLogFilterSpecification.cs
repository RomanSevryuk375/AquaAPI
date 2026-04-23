using Notification.Domain.Entities;
using Notification.Domain.SpecificationParams;

namespace Notification.Domain.Specifications;

public class MaintenanceLogFilterSpecification 
    : BaseSpecification<MaintenanceLogEntity>
{
    public MaintenanceLogFilterSpecification(
        MaintenanceLogSpecificationParams @params) 
        : base(data => 
            (!@params.UserId.HasValue || data.UserId == @params.UserId.Value) 
            && (!@params.AquariumId.HasValue || data.AquariumId == @params.AquariumId.Value)
            && (!@params.ActionDateFrom.HasValue || data.ActionDate >= @params.ActionDateFrom.Value)
            && (!@params.ActionDateTo.HasValue || data.ActionDate <= @params.ActionDateTo.Value))
    {
    }
}
