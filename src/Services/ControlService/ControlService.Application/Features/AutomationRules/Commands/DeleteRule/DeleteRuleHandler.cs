using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Domain.Interfaces;
using MediatR;
using ZiggyCreatures.Caching.Fusion;

namespace Control.Application.Features.AutomationRules.Commands.DeleteRule;

public sealed class DeleteRuleHandler(
    IAutomationRuleRepository ruleRepository,
    IFusionCache cache)
    : IRequestHandler<DeleteRuleCommand, Result>
{
    public async Task<Result> Handle(
        DeleteRuleCommand request,
        CancellationToken cancellationToken)
    {
        await ruleRepository.DeleteAsync(request.RuleId, cancellationToken);

        await cache.RemoveAsync(CacheKeys.Rule(request.UserId, request.RuleId), token: cancellationToken);

        return Result.Success();
    }
}
