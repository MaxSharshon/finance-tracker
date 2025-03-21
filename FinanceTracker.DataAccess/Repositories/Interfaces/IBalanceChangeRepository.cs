using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IBalanceChangeRepository : IRepository<BalanceChange>
{
    Task<IEnumerable<BalanceChange>> GetUnusedAsync();
}