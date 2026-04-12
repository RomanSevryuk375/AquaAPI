using Contracts.Enums;
using Control.Domain.Interfaces;

namespace Control.Domain.Entities;

public class AutomationRuleEntity : IEntity
{
    private AutomationRuleEntity(
        Guid id, 
        Guid aquariumId, 
        Guid sensorId, 
        Guid relayId, 
        RuleConditionEnum condition, 
        double threshold, 
        double hysteresis, 
        RuleActionEnum action, 
        DateTime createdAt)
    {
        Id = id;
        AquariumId = aquariumId;
        SensorId = sensorId;
        RelayId = relayId;
        Condition = condition;
        Threshold = threshold;
        Hysteresis = hysteresis;
        Action = action;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid AquariumId { get; private set; }
    public Guid SensorId { get; private set; }
    public Guid RelayId { get; private set; }
    public RuleConditionEnum Condition { get; private set; }
    public double Threshold { get; private set; }
    public double Hysteresis { get; private set; }
    public RuleActionEnum Action { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (AutomationRuleEntity? rule, List<string> errors) Create(
        Guid aquariumId,
        Guid sensorId,
        Guid relayId,
        RuleConditionEnum condition,
        double threshold,
        double hysteresis,
        RuleActionEnum action)
    {
        var errors = new List<string>();

        if (sensorId == Guid.Empty)
        {
            errors.Add("sensorId must not be empty.");
        }

        if (aquariumId == Guid.Empty)
        {
            errors.Add("aquariumId must not be empty.");
        }

        if (relayId == Guid.Empty)
        {
            errors.Add("relayId must not be empty.");
        }

        if (hysteresis < 0)
        {
            errors.Add("hysteresis must not be less then zero.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var rule = new AutomationRuleEntity(
            Guid.NewGuid(),
            aquariumId,
            sensorId,
            relayId,
            condition,
            threshold,
            hysteresis,
            action,
            DateTime.UtcNow);

        return (rule, errors);
    }

    public List<string>? Update (
        RuleConditionEnum condition,
        double threshold,
        double hysteresis,
        RuleActionEnum action)
    {
        var errors = new List<string>();

        if (hysteresis < 0)
        {
            errors.Add("hysteresis must not be less then zero.");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        Condition = condition;
        Threshold = threshold;
        Hysteresis = hysteresis;
        Action = action;

        return null;
    }

    public void SetCondition(RuleConditionEnum condition)
    {
        if (Condition == condition)
        {
            return;
        }

        Condition = condition;
    }

    public void SetAction(RuleActionEnum action)
    {
        if (Action == action)
        {
            return;
        }

        Action = action;
    }

    public void SetThreshold(double threshold)
    {
        Threshold = threshold;
    }

    public string? SetHysteresis(double hysteresis)
    {
        if (hysteresis < 0)
        {
            return("hysteresis must not be less then zero.");
        }

        Hysteresis = hysteresis;

        return null;
    }
}
