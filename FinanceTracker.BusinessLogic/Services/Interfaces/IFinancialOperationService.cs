using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IFinancialOperationService
{
    Task<IEnumerable<FinancialOperationDto>> GetAllAsync();
    Task<FinancialOperationDto> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(FinancialOperationDto operationDto);
    Task UpdateAsync(FinancialOperationDto operationDto);
    Task RemoveAsync(Guid id);
}