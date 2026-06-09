using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Contracts.Budgets;

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
