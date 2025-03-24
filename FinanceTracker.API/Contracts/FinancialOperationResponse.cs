namespace FinanceTracker.API.Contracts;

public record FinancialOperationResponse(Guid Id, DateTime Date, Guid BalanceChangeId);