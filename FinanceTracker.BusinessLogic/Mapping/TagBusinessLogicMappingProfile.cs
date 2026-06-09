using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Mapping;

public class TagBusinessLogicMappingProfile : Profile
{
    public TagBusinessLogicMappingProfile()
    {
        CreateMap<Tag, TagDto>().ReverseMap();
    }
}
