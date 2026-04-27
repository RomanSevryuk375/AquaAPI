using Device.Application.DTOs.Telemetry;
using FluentValidation;

namespace Device.Application.DTOs.Validators.TelemtryValidators;

public class TelemetryItemRequestValidator 
    : AbstractValidator<TelemetryItemRequest>
{
    public TelemetryItemRequestValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty();

        RuleFor(x => x.Value)
            .NotEmpty();

        RuleFor(x => x.ExternalMessageId)
            .NotEmpty();

        RuleFor(x => x.RecordedAt)
            .NotEmpty()
            .Must(BeInPast).WithMessage("RecordedAt can not be in future.");

    }

    private bool BeInPast(DateTime recordedAt)
    {
        return recordedAt < DateTime.UtcNow.AddMinutes(5);
    }
}
