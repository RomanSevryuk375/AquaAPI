using Device.Application.DTOs.Telemetry;
using FluentValidation;

namespace Device.Application.DTOs.Validators;

public class TelemetryBatchRequestValidator : AbstractValidator<TelemetryBatchRequest>
{
    public TelemetryBatchRequestValidator()
    {
        RuleFor(x => x.MacAddress)
            .NotEmpty().WithMessage("Controller MacAddress must not be empty or white space.")
            .Matches(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")
            .WithMessage("Invalid MacAddress format.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Batch items must not be empty")
            .Must(x => x.Count > 50).WithMessage("Maximum batch size is 50 items.");

        RuleForEach(x => x.Items).SetValidator(new TelemetryItemRequestValidator());
    }
}
