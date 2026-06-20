using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class NotificationService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<Notification> validator) 
    : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetAllAsync(Guid userId)
    {
        var notifications = await unitOfWork.Notifications.GetByUserAsync(userId);
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }
    
    public async Task<NotificationDto> GetByIdAsync(Guid id, Guid userId)
    {
        return mapper.Map<NotificationDto>(await GetEntityByIdAsync(id, userId));
    }

    public async Task<Guid> AddAsync(NotificationDto notificationDto, Guid userId)
    {
        var notification = mapper.Map<Notification>(notificationDto);
        notification.UserId = userId;
        notification.CreatedAt = DateTime.UtcNow;

        validator.EnsureValid(notification);
        
        await unitOfWork.Notifications.AddAsync(notification);
        await unitOfWork.CompleteAsync();

        return notification.Id;
    }

    public async Task UpdateAsync(NotificationDto notificationDto, Guid userId)
    {
        var existingNotification = await GetEntityByIdAsync(notificationDto.Id, userId);

        existingNotification.Message = notificationDto.Message;
        existingNotification.IsRead = notificationDto.IsRead;

        validator.EnsureValid(existingNotification);
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        var notification = await GetEntityByIdAsync(id, userId);
        unitOfWork.Notifications.Remove(notification);
        await unitOfWork.CompleteAsync();
    }
    
    private async Task<Notification> GetEntityByIdAsync(Guid id, Guid userId)
    {
        return await unitOfWork.Notifications.GetByIdAsync(id, userId)
               ?? throw new KeyNotFoundException($"A {nameof(Notification)} with ID {id} not found.");
    }
}
