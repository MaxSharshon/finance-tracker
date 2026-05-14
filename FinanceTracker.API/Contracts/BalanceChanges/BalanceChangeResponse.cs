namespace FinanceTracker.API.Contracts.BalanceChanges;

public record BalanceChangeResponse(Guid Id, string OperationType, decimal Amount);