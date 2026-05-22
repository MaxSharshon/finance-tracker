using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IFinancialOperationService : IScopedCrudService<FinancialOperationDto, Guid>
{
    Task<IEnumerable<FinancialOperationDto>> GetAllAsync(Guid userId);
}