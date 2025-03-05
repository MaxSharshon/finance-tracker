using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IBalanceChangeService
{
    IEnumerable<BalanceChange> GetAll();
    BalanceChange GetById(Guid id);
    void Add(BalanceChange balanceChange);
    void Update(BalanceChange balanceChange);
    void Remove(Guid id);
}