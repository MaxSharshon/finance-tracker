using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserAsync(Guid userId);
}
