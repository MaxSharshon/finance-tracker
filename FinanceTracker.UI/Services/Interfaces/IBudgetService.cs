using FinanceTracker.Contracts.Budgets;

namespace FinanceTracker.UI.Services.Interfaces;

public interface IBudgetService : IService<BudgetRequest, BudgetResponse>
{
    Task<IEnumerable<BudgetMemberResponse>> GetMembersAsync(Guid budgetId);
    Task<HttpResponseMessage> AddMemberAsync(Guid budgetId, BudgetMemberRequest request);
    Task<HttpResponseMessage> RemoveMemberAsync(Guid budgetId, Guid memberUserId);
}
