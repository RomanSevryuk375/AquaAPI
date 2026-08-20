using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Control.Application.Constants;
using Control.Application.Interfaces;

namespace Control.Application.Features.AutomationRules.Queries.GetRuleById;

public sealed record GetRuleByIdQuery
    : ICachedQuery<Result<AutomationRuleDto>>, IRuleBoundRequest
{
    public Guid RuleId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.Rule(UserId, RuleId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
