namespace FinanceTracker.API.Contracts;

public record BalanceChangeRequest(string OperationType,decimal Amount);