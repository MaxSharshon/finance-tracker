namespace FinanceTracker.API.Contracts.FinancialOperations;

public record FinancialOperationRequest(
    Guid CategoryId,
    Guid? BudgetId,
    decimal Amount,
    DateTime Date,
    string Description,
    List<Guid> TagIds);