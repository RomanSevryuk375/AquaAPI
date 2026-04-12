using Contracts.Enums;

namespace Control.Application.DTOs.AutomationRule;

public record AutomationRuleUpdateRequestDto
{
    public RuleConditionEnum Condition { get; private set; }
    public double Threshold { get; private set; }
    public double Hysteresis { get; private set; }
    public RuleActionEnum Action { get; private set; }
}
