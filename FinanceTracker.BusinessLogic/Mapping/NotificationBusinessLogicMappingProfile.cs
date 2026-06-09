using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class NotificationBusinessLogicMappingProfile : Profile
{
    public NotificationBusinessLogicMappingProfile()
    {
        CreateMap<Notification, NotificationDto>().ReverseMap();
    }
}