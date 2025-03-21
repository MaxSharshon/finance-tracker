using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IBalanceChangeService
{
    Task<IEnumerable<BalanceChangeDto>> GetAllAsync();
    Task<BalanceChangeDto> GetById(Guid id);
    Task<IEnumerable<BalanceChangeDto>> GetUnusedAsync();
    Task<Guid> AddAsync(BalanceChangeDto balanceChangeDto);
    Task UpdateAsync(BalanceChangeDto balanceChangeDto);
    Task RemoveAsync(Guid id);
}