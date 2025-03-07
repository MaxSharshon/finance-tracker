using AutoMapper;
using FinanceTracker.API.Contracts;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Mapping;

public class ApiMapper : Profile
{
    public ApiMapper()
    {
        CreateMap<BalanceChangeDto, BalanceChangeResponse>()
            .ForMember(dest => dest.OperationType, opt => 
                opt.MapFrom(src => Enum.GetName(src.OperationType)));

        CreateMap<BalanceChangeRequest, BalanceChangeDto>()
            .ForMember(dest => dest.OperationType, opt => 
                    opt.MapFrom(src => ConvertOperationType(src.OperationType)));
    }
    
    private static OperationType ConvertOperationType(string operationType)
    {
        if (Enum.TryParse<OperationType>(operationType, true, out var result))
            return result;

        throw new ArgumentException($"Invalid operation type: {operationType}");
    }
}