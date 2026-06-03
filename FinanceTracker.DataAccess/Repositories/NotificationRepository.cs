using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class NotificationRepository(FinanceTrackerContext context) : Repository<Notification>(context), INotificationRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<IEnumerable<Notification>> GetByUserAsync(Guid userId)
    {
        return await FinanceTrackerContext.Notifications
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(Guid id, Guid userId)
    {
        return await FinanceTrackerContext.Notifications
            .FirstOrDefaultAsync(notification => notification.Id == id && notification.UserId == userId);
    }
}
