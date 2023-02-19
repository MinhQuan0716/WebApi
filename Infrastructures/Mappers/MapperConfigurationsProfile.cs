﻿using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.SyllabusModels;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Microsoft.AspNetCore.Session;

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

            CreateMap<UpdateLectureDTO, Lecture>()
                .ForMember(dest => dest.LectureName, opt => opt.MapFrom(src => src.LectureName))
                .ForMember(dest => dest.OutputStandards, opt => opt.MapFrom(src => src.OutputStandards))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.DeliveryType, opt => opt.MapFrom(src => src.DeliveryType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LectureID))
                .ReverseMap();
        
             

            CreateMap<UpdateUnitDTO, Unit>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitName))
                .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => src.TotalTime))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UnitID))
                .ForMember(dest => dest.SyllabusID, opt => opt.MapFrom(src => src.syllabusId))
                .ReverseMap();

            CreateMap<UpdateSyllabusDTO, Syllabus>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CourseObjective, opt => opt.MapFrom(src => src.CourseObject))
                .ForMember(dest => dest.SyllabusName, opt => opt.MapFrom(src => src.SyllabusName))
                .ForMember(dest => dest.TechRequirements, opt => opt.MapFrom(src => src.TechRequirement))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units.Select(x => new Unit
                {
                    Id = x.UnitID.Value,
                    UnitName = x.UnitName,
                    TotalTime = x.TotalTime,
                    SyllabusID = x.syllabusId.Value
                })))
                .ReverseMap();

            CreateMap<UpdateUnitLectureDTO, DetailUnitLecture>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LectureID, opt => opt.MapFrom(src => src.LectureId))
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId)).ReverseMap();
        }
    }
}
