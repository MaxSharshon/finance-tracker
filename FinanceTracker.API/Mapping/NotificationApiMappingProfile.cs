using AutoMapper;
using FinanceTracker.API.Contracts.Notifications;
using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Mapping;

public class NotificationApiMappingProfile : Profile
{
    public NotificationApiMappingProfile()
    {
        CreateMap<NotificationRequest, NotificationDto>();

        CreateMap<NotificationDto, NotificationResponse>();
    }
}
