namespace FinanceTracker.API.Contracts;

public record FinancialOperationResponse(Guid Id, string Date, Guid BalanceChangeId);