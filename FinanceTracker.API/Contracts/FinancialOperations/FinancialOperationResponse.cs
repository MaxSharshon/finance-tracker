namespace FinanceTracker.API.Contracts.FinancialOperations;

public record FinancialOperationResponse(
    Guid Id,
    Guid? UserId,
    Guid CategoryId,
    Guid? BudgetId,
    decimal Amount,
    DateTime Date,
    string Description,
    List<Guid> TagIds);