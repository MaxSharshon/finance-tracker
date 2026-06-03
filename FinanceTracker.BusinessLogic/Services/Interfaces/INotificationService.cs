using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface INotificationService : IScopedCrudService<NotificationDto, Guid>
{
    Task<IEnumerable<NotificationDto>> GetAllAsync(Guid userId);
}
