using System.Linq.Expressions;

namespace Telemetry.Domain.Interfaces;

public abstract class BaseSpecification<T>(Expression<Func<T, bool>> criteria)
{
    public Expression<Func<T, bool>> Criteria { get; } = criteria;
}
