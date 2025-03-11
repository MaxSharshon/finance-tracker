using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class BusinessLogicMapper : Profile
{
    public BusinessLogicMapper()
    {
        CreateMap<BalanceChange, BalanceChangeDto>().ReverseMap();
        CreateMap<FinancialOperation, FinancialOperationDto>().ReverseMap();
    }
}