﻿using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.SyllabusModels;

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

            CreateMap<RegisterDTO, User>()
                .ForMember(rd => rd.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(rd => rd.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(rd => rd.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();

            CreateMap<AddUserManually, User>()
               .ForMember(destinationMember => destinationMember.UserName, options => options.MapFrom(src => src.UserName))
               .ForMember(destinationMember => destinationMember.PasswordHash, options => options.MapFrom(src => src.Pass))
              .ForMember(destinationMember => destinationMember.FullName, options => options.MapFrom(src => src.FullName))
              .ForMember(destinationMember => destinationMember.Email, options => options.MapFrom(src => src.Email))
              .ForMember(destinationMember => destinationMember.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth))
              .ForMember(destinationMember => destinationMember.Gender, options => options.MapFrom(src => src.Gender))
              .ForMember(destinationMember => destinationMember.RoleId, options => options.MapFrom(src => src.RoleId)).ReverseMap();

            CreateMap<ResetPasswordDTO, User>()
                .ForMember(rp => rp.PasswordHash, opt => opt.MapFrom(src => src.NewPassword)).ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(desc => desc._Id, src => src.MapFrom(u => u.Id))
                .ReverseMap();

            CreateMap<SyllabusGeneralDTO, Syllabus>()
           .ForMember(ss => ss.SyllabusName, ss => ss.MapFrom(src => src.SyllabusName))
            .ForMember(ss => ss.Duration, ss => ss.MapFrom(src => src.Duration))
             .ForMember(ss => ss.CourseObjective, ss => ss.MapFrom(src => src.CourseObject))
               .ForMember(ss => ss.TechRequirements, ss => ss.MapFrom(src => src.TechRequirement))
               .ForMember(ss => ss.Duration, ss => ss.MapFrom(src => src.Duration))
               .ReverseMap();
        }
    }
}
