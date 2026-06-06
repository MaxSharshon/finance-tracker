using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class BalanceChangeBusinessLogicMappingProfile : Profile
{
    public BalanceChangeBusinessLogicMappingProfile()
    {
        CreateMap<BalanceChange, BalanceChangeDto>().ReverseMap();
    }
}
