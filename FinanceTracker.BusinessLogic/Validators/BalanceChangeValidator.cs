using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class BalanceChangeValidator : AbstractValidator<BalanceChange>
{
    public BalanceChangeValidator()
    {
        RuleFor(change => change.OperationType)
            .IsInEnum().WithMessage("{PropertyName} is invalid.");
        
        RuleFor(change => change.Amount)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than {ComparisonValue}}.");
    }
}