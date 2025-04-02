using FinanceTracker.UI.Models;

namespace FinanceTracker.UI.Services.Interfaces;

public interface IBalanceChangeService : IService<BalanceChangeRequest, BalanceChangeResponse>
{
    Task<IEnumerable<BalanceChangeResponse>> GetUnusedAsync();
}