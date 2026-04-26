using Device.Application.DTOs.Telemetry;
using FluentValidation;

namespace Device.Application.DTOs.Validators;

public class TelemetryItemRequestValidator : AbstractValidator<TelemetryItemRequest>
{
    public TelemetryItemRequestValidator()
    {
        RuleFor(x => x.SensorId)
            .NotEmpty().WithMessage("SensorId must not be empty.")
            .NotEqual(Guid.Empty).WithMessage("SensorId cannot be an empty Guid.");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value must not be empty.");

        RuleFor(x => x.ExternalMessageId)
            .NotEmpty().WithMessage("SensorId must not be empty.");

        RuleFor(x => x.RecordedAt)
            .NotEmpty().WithMessage("RecordedAt must not be empty.")
            .Must(BeInPast).WithMessage("RecordedAt can not be in future.");

    }

    private bool BeInPast(DateTime recordedAt)
    {
        return recordedAt < DateTime.UtcNow.AddMinutes(5);
    }
}
