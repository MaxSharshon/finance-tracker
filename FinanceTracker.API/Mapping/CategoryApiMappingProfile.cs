using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Mapping;

public class CategoryApiMappingProfile : Profile
{
    public CategoryApiMappingProfile()
    {
        CreateMap<CategoryRequest, CategoryDto>()
            .ForMember(dest => dest.OperationType,
                opt => opt.MapFrom(src => ConvertOperationType(src.OperationType)));

        CreateMap<CategoryDto, CategoryResponse>()
            .ForMember(dest => dest.OperationType,
                opt => opt.MapFrom(src => Enum.GetName(src.OperationType) ?? src.OperationType.ToString()));

        CreateMap<CategorySuggestionDto, CategorySuggestionResponse>()
            .ForMember(dest => dest.OperationType,
                opt => opt.MapFrom(src => Enum.GetName(src.OperationType) ?? src.OperationType.ToString()));
    }

    private static OperationType ConvertOperationType(string operationType)
    {
        return Enum.TryParse<OperationType>(operationType, true, out var result)
            ? result
            : throw new ArgumentException($"Invalid operation type: {operationType}");
    }
}
