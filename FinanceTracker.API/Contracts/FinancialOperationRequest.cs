namespace FinanceTracker.API.Contracts;

public record FinancialOperationRequest(
    Guid? UserId,
    Guid? CategoryId,
    Guid? BudgetId,
    decimal Amount,
    string OperationType, 
    DateTime Date,
    string Description,
    List<Guid> TagIds);