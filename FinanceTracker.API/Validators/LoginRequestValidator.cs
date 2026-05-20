using FinanceTracker.API.Contracts.Auth;
using FluentValidation;

namespace FinanceTracker.API.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("{PropertyName} is invalid.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
