// Ignore Spelling: Validator

using BuildingBlocks.Domain.Constants;
using FluentValidation;

namespace IdentityService.Application.Features.Profile.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(IdentityConstants.PasswordLength)
            .WithMessage(ValidationMessages.PasswordMinLength);
    }
}
