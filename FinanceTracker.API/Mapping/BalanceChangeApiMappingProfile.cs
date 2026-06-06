using AutoMapper;
using FinanceTracker.API.Contracts.BalanceChanges;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Mapping;

public class BalanceChangeApiMappingProfile : Profile
{
    public BalanceChangeApiMappingProfile()
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
        return Enum.TryParse<OperationType>(operationType, true, out var result)
            ? result
            : throw new ArgumentException($"Invalid operation type: {operationType}");
    }
}
