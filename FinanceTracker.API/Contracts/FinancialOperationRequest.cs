namespace FinanceTracker.API.Contracts;

public record FinancialOperationRequest(string Date, Guid BalanceChangeId);