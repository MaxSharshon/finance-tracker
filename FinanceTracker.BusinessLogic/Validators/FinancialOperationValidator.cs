using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class FinancialOperationValidator : AbstractValidator<FinancialOperation>
{
    public FinancialOperationValidator()
    {
        RuleFor(operation => operation.Date)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(operation => operation.BalanceChange)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .SetValidator(new BalanceChangeValidator());
    }
}