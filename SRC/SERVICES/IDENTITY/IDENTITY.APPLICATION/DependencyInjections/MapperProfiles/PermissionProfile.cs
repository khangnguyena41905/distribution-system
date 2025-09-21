using AutoMapper;
using IDENTITY.APPLICATION.Dtos.PermissionDtos;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.DependencyInjections.MapperProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<Permission, PermissionDto>();

        CreateMap<ActionInFunction, ActionInFunctionDto>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

        CreateMap<Function, FunctionDto>()
            .ForMember(dest => dest.ActionInFunctions, opt => opt.MapFrom(src => src.ActionInFunctions));
    }
}