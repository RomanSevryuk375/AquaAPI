// Ignore Spelling: Dto

using BuildingBlocks.Domain.Enums;

namespace Control.Application.DTOs.AutomationRules;

public sealed record RuleConditionRequestDto
{
    public Guid SensorId { get; init; }
    public Condition Condition { get; init; }
    public double Threshold { get; init; }
    public double Hysteresis { get; init; }
}
