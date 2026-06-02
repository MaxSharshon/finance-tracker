using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class BudgetValidator : AbstractValidator<Budget>
{
    public BudgetValidator()
    {
        RuleFor(budget => budget.OwnerUserId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        
        RuleFor(budget => budget.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(128).WithMessage("{PropertyName} is too long.");
        
        RuleFor(budget => budget.LimitAmount)
            .GreaterThan(0).When(budget => budget.LimitAmount.HasValue)
            .WithMessage("{PropertyName} must be greater than {ComparisonValue}.");

        RuleFor(budget => budget.EndDate)
            .GreaterThanOrEqualTo(budget => budget.StartDate)
            .When(budget => budget.StartDate.HasValue && budget.EndDate.HasValue)
            .WithMessage("{PropertyName} must be greater than or equal to StartDate.");
    }
}
