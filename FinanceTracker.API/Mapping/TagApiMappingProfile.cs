using AutoMapper;
using FinanceTracker.API.Contracts.Tags;
using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Mapping;

public class TagApiMappingProfile : Profile
{
    public TagApiMappingProfile()
    {
        CreateMap<TagRequest, TagDto>();

        CreateMap<TagDto, TagResponse>();
    }
}
