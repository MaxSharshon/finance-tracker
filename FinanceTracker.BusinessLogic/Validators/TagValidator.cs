using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class TagValidator : AbstractValidator<Tag>
{
    public TagValidator()
    {
        RuleFor(tag => tag.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        
        RuleFor(tag => tag.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(64).WithMessage("{PropertyName} is too long.");
    }
}
