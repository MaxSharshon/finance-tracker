using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(category => category.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(128).WithMessage("{PropertyName} is too long.");
        
        RuleFor(category => category.OperationType)
            .IsInEnum().WithMessage("{PropertyName} is invalid.");
    }
}
