using System.Linq.Expressions;

namespace Notification.Domain.Specifications;

public abstract class BaseSpecification<T>(Expression<Func<T, bool>> criteria)
{
    public Expression<Func<T, bool>> Criteria = criteria;
}
