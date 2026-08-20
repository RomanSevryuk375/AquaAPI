using BuildingBlocks.Domain.Abstractions;
using Control.Application.Interfaces;

namespace Control.Application.Features.AutomationRules.Commands.DeleteRule;

public sealed record DeleteRuleCommand
    : ICommand, IRuleBoundRequest
{
    public Guid UserId { get; init; }
    public Guid RuleId { get; init; }
}
