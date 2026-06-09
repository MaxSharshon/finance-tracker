using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Contracts.Notifications;

namespace FinanceTracker.API.Mapping;

public class NotificationApiMappingProfile : Profile
{
    public NotificationApiMappingProfile()
    {
        CreateMap<NotificationRequest, NotificationDto>();

        CreateMap<NotificationDto, NotificationResponse>();
    }
}
