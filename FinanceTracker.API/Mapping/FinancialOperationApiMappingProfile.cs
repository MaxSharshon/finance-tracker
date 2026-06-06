using AutoMapper;
using FinanceTracker.API.Contracts.FinancialOperations;
using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Mapping;

public class FinancialOperationApiMappingProfile : Profile
{
    public FinancialOperationApiMappingProfile()
    {
        CreateMap<FinancialOperationRequest, FinancialOperationDto>()
            .ForMember(dest => dest.TagIds,
                opt => opt.MapFrom(src => src.TagIds));

        CreateMap<FinancialOperationDto, FinancialOperationResponse>()
            .ForMember(dest => dest.TagIds,
                opt => opt.MapFrom(src => src.TagIds));

        CreateMap<FinancialOperationFilterRequest, FinancialOperationFilterDto>();
    }
}
