using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Contracts.Tags;

namespace FinanceTracker.API.Mapping;

public class TagApiMappingProfile : Profile
{
    public TagApiMappingProfile()
    {
        CreateMap<TagRequest, TagDto>();

        CreateMap<TagDto, TagResponse>();
    }
}
