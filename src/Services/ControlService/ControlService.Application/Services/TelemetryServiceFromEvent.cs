using Contracts.Enums;
using Contracts.Events.RelayEvents;
using Contracts.Events.TelemetryEvents;
using Control.Application.Interfaces;
using Control.Domain.Factories;
using Control.Domain.Interfaces;
using MassTransit;

namespace Control.Application.Services;

public class TelemetryServiceFromEvent(
    IAutomationRuleRepository ruleRepository,
    IRelayRepository relayRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint) : ITelemetryServiceFromEvent
{
    public async Task ProcessTelemetryAsync(
        TelemetryReceivedEvent telemetry,
        CancellationToken cancellationToken)
    {
        var rules = await ruleRepository
            .GetBySensorIdAsync(telemetry.SensorId, cancellationToken);

        if (rules == null)
        {
            return;
        }

        foreach (var rule in rules)
        {
            var relay = await relayRepository
                .GetByIdAsync(rule.RelayId, cancellationToken);

            if (relay == null || relay.IsManual)
            {
                continue;
            }

            var evalute = RuleEvaluatorFactory.Create(rule.Condition);

            var isMet = evalute.Evaluate(telemetry.Value, rule.Threshold, rule.Hysteresis);

            if (isMet == null)
            {
                continue;
            }

            bool targetState = isMet.Value
                ? rule.Action == RuleActionEnum.SwitchOn
                : rule.Action == RuleActionEnum.SwitchOff;

            if (relay.IsActive == targetState)
            {
                continue;
            }

            relay.SetState(targetState);

            await relayRepository.UpdateAsync(relay, cancellationToken);

            await publishEndpoint.Publish(new ChangeRelayStateCommand
            {
                RelayId = relay.Id,
                IsActive = targetState,
            }, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
