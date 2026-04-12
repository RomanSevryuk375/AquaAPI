using Contracts.Enums;

namespace Control.Application.DTOs.AutomationRule;

public record AutomationRuleRequestDto
{
    public Guid AquariumId { get; init; }
    public Guid SensorId { get; init; }
    public Guid RelayId { get; init; }
    public RuleConditionEnum Condition { get; init; }
    public double Threshold { get; init; }
    public double Hysteresis { get; init; }
    public RuleActionEnum Action { get; init; }
}
