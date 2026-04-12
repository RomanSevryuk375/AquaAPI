using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Domain.Specifications;

public class ActiveScheduleSpecification : BaseSpecification<ScheduleEntity>
{
    public ActiveScheduleSpecification() : base(data => data.IsEnable)
    {
    }
}
