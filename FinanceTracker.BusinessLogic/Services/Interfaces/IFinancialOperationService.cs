using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IFinancialOperationService
{
    Task<IEnumerable<FinancialOperationDto>> GetAllAsync();
    Task<FinancialOperationDto> GetById(Guid id);
    Task<Guid> AddAsync(FinancialOperationDto financialOperationDto);
    Task UpdateAsync(FinancialOperationDto financialOperationDto);
    Task RemoveAsync(Guid id);
}