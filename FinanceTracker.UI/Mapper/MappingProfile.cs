using AutoMapper;
using FinanceTracker.UI.Models;

namespace FinanceTracker.UI.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BalanceChangeRequest, BalanceChangeResponse>().ReverseMap();
        
        CreateMap<FinancialOperationRequest, FinancialOperationResponse>().ReverseMap();
    }
}