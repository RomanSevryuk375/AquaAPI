using Control.Domain.Interfaces;

namespace Control.Domain.Strategies;

public class EqualConditionEvaluator : IRuleEvaluator
{
    public bool? Evaluate(double currentValue, double threshold, double hysteresis)
    {
        if ((currentValue <= threshold - hysteresis) && (currentValue > threshold + hysteresis))
        {
            return true;
        }

        return false;
    }
}
