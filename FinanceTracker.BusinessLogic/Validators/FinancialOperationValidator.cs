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

        RuleFor(operation => operation.BalanceChangeId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}