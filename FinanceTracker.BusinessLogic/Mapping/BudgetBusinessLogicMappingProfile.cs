using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class BudgetBusinessLogicMappingProfile : Profile
{
    public BudgetBusinessLogicMappingProfile()
    {
        CreateMap<Budget, BudgetDto>().ReverseMap();
        
        CreateMap<BudgetUser, BudgetMemberDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
            .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User != null ? src.User.DisplayName : string.Empty));
    }
}
