using FinanceTracker.Contracts.Enums;

namespace FinanceTracker.Contracts.FinancialOperations;

public record FinancialOperationFilterRequest(
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? CategoryId,
    Guid? BudgetId,
    OperationType? OperationType);
