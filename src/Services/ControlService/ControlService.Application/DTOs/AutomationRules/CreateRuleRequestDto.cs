// Ignore Spelling: Dto

using BuildingBlocks.Domain.Enums;

namespace Control.Application.DTOs.AutomationRules;

public sealed record CreateRuleRequestDto
{
    public Guid EcosystemId { get; init; }
    public Guid RelayId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Operator Operator { get; init; }
    public RuleAction Action { get; init; }
    public bool IsActive { get; init; }
    public List<RuleConditionRequestDto> Conditions { get; init; } = [];
}
