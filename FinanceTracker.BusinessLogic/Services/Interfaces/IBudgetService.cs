using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IBudgetService : IScopedCrudService<BudgetDto, Guid>
{
    Task<IEnumerable<BudgetDto>> GetAllAsync(Guid userId);
    Task<IEnumerable<BudgetMemberDto>> GetMembersAsync(Guid budgetId, Guid userId);
    Task AddMemberAsync(Guid budgetId, Guid memberUserId, Guid userId);
    Task RemoveMemberAsync(Guid budgetId, Guid memberUserId, Guid userId);
}
