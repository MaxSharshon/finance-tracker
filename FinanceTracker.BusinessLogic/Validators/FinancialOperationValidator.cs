using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class FinancialOperationValidator : AbstractValidator<FinancialOperation>
{
    public FinancialOperationValidator()
    {
        RuleFor(operation => operation.Date)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("{PropertyName} cannot be in the future.");

        RuleFor(operation => operation.CategoryId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        
        RuleFor(operation => operation.Amount)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}}.");
        
        RuleFor(operation => operation.Description)
            .MaximumLength(512).WithMessage("{PropertyName} is too long.");
    }
}
