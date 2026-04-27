using Contracts.Constants;
using Device.Application.DTOs.Telemetry;
using FluentValidation;

namespace Device.Application.DTOs.Validators.TelemtryValidators;

public class TelemetryBatchRequestValidator 
    : AbstractValidator<TelemetryBatchRequest>
{
    public TelemetryBatchRequestValidator()
    {
        RuleFor(x => x.MacAddress)
            .NotEmpty()
            .Matches(ControllerConstants.MacAddressRegex)
            .WithMessage("Invalid MacAddress format.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(x => x.Count <= 50).WithMessage("Maximum batch size is 50 items.");

        RuleForEach(x => x.Items).SetValidator(new TelemetryItemRequestValidator());
    }
}
