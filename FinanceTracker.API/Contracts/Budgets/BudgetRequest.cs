namespace FinanceTracker.API.Contracts.Budgets;

public record BudgetRequest(
    string Name,
    decimal? LimitAmount,
    DateTime? StartDate,
    DateTime? EndDate);
