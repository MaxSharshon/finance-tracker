namespace FinanceTracker.API.Contracts;

public record BalanceChangeResponse(Guid Id, string OperationType,decimal Amount);