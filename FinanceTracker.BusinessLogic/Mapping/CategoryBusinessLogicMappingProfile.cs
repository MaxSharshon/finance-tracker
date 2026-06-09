using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class CategoryBusinessLogicMappingProfile : Profile
{
    public CategoryBusinessLogicMappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}
