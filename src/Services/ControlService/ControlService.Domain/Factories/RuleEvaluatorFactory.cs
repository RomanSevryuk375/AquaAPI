using Contracts.Enums;
using Control.Domain.Interfaces;
using Control.Domain.Strategies;

namespace Control.Domain.Factories;

public static class RuleEvaluatorFactory
{
    public static IRuleEvaluator Create (RuleConditionEnum condition)
    {
        return condition switch
        {
            RuleConditionEnum.Equal => new EqualConditionEvaluator(),
            RuleConditionEnum.Greater => new GreaterConditionEvaluator(),
            RuleConditionEnum.GreaterOrEqual => new GreaterOrEqualConditionEvaluator(),
            RuleConditionEnum.Less => new LessConditionEvaluator(),
            RuleConditionEnum.LessOrEqual => new LessOrEqualConditionEvaluator(),

            _ => throw new ArgumentOutOfRangeException($"Condition {condition} is not supported")
        };
    }
}
