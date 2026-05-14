namespace FinanceTracker.API.Contracts.BalanceChanges;

public record BalanceChangeRequest(string OperationType, decimal Amount);