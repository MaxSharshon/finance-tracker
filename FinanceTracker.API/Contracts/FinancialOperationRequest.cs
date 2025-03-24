namespace FinanceTracker.API.Contracts;

public record FinancialOperationRequest(DateTime Date, Guid BalanceChangeId);