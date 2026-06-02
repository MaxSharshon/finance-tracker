namespace FinanceTracker.API.Contracts.Budgets;

public record BudgetResponse(
    Guid Id, 
    string Name, 
    decimal? LimitAmount, 
    DateTime? StartDate, 
    DateTime? EndDate);
