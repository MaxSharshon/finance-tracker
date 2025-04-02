namespace FinanceTracker.UI.Models;

public record FinancialOperationResponse(Guid Id, DateTime Date, Guid BalanceChangeId);