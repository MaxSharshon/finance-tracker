using AutoMapper;
using FinanceTracker.API.Contracts.Budgets;
using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Mapping;

public class BudgetApiMappingProfile : Profile
{
    public BudgetApiMappingProfile()
    {
        CreateMap<BudgetRequest, BudgetDto>();

        CreateMap<BudgetDto, BudgetResponse>();

        CreateMap<BudgetMemberDto, BudgetMemberResponse>();
    }
}
