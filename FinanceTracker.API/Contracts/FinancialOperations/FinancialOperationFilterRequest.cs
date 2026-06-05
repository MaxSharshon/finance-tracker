using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Contracts.FinancialOperations;

public record FinancialOperationFilterRequest(
    DateTime? StartDate,
    DateTime? EndDate,
    Guid? CategoryId,
    Guid? BudgetId,
    OperationType? OperationType);
