using FinanceTracker.Contracts.Auth;
using FluentValidation;

namespace FinanceTracker.API.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("{PropertyName} is invalid.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(8).WithMessage("{PropertyName} must be at least {MinLength} characters long.");

        RuleFor(request => request.DisplayName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(128).WithMessage("{PropertyName} is too long.");
    }
}
