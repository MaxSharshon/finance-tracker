namespace FinanceTracker.API.Contracts.Budgets;

public record BudgetMemberResponse(Guid UserId, string Email, string DisplayName);
