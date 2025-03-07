using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IBalanceChangeService
{
    IEnumerable<BalanceChangeDto> GetAll();
    BalanceChangeDto GetById(Guid id);
    Guid Add(BalanceChangeDto balanceChangeDto);
    void Update(BalanceChangeDto balanceChangeDto);
    void Remove(Guid id);
}