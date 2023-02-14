using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.UserViewModels;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));

            // Create Mapping UpdateDTO -- User
            CreateMap<UpdateDTO, User>()
                .ForMember(uu => uu.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(uu => uu.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(uu => uu.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(uu => uu.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(uu => uu.RoleId, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(uu => uu.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(uu => uu.Id, opt => opt.MapFrom(src => src.UserID))
                .ReverseMap();
        }
    }
}
