using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IBudgetService : IScopedCrudService<BudgetDto, Guid>
{
    Task<IEnumerable<BudgetDto>> GetAllAsync(Guid userId);
}
