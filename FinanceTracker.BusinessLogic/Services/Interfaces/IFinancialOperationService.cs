using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IFinancialOperationService : ICrudService<FinancialOperationDto>
{
    Task<IEnumerable<FinancialOperationDto>> GetAllAsync();
}