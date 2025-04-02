namespace FinanceTracker.UI.Models;

public record BalanceChangeResponse(Guid Id, decimal Amount, string OperationType);