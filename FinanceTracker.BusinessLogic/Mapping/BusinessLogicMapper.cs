using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class BusinessLogicMapper : Profile
{
    public BusinessLogicMapper()
    {
        CreateMap<BalanceChange, BalanceChangeDto>().ReverseMap();

        CreateMap<FinancialOperation, FinancialOperationDto>()
            .ForMember(dest => dest.TagIds,
                opt => opt.MapFrom(src => src.OperationTags.Select(ot => ot.TagId)));

        CreateMap<FinancialOperationDto, FinancialOperation>()
            .ForMember(dest => dest.OperationTags, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Budget, opt => opt.Ignore())
            .ForMember(dest => dest.BalanceChange, opt => opt.Ignore());
    }
}
