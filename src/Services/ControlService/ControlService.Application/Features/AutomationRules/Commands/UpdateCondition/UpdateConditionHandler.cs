using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.AutomationRules.Commands.UpdateCondition;

public sealed class UpdateConditionHandler(
    IAutomationRuleRepository ruleRepository,
    IFusionCache cache)
    : IRequestHandler<UpdateConditionCommand, Result>
{
    public async Task<Result> Handle(
        UpdateConditionCommand request,
        CancellationToken cancellationToken)
    {
        AutomationRule? rule = await ruleRepository.GetByIdWithConditionsAsync(
            request.RuleId, cancellationToken);
        if (rule is null)
        {
            return Result.Failure(Error.NotFound<AutomationRule>(
                $"Rule {request.RuleId} not found"));
        }

        RuleCondition? condition = rule.Conditions.FirstOrDefault(x => x.Id == request.ConditionId);
        if (condition is null)
        {
            return Result.Failure(Error.NotFound<RuleCondition>(
                $"Condition {request.ConditionId} not found."));
        }

        Result updateResult = condition.Update(request.Condition, request.Threshold, request.Hysteresis);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await cache.RemoveAsync(CacheKeys.Rule(request.UserId, request.RuleId), token: cancellationToken);

        return Result.Success();
    }
}
