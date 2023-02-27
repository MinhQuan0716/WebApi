using AutoMapper;
using Application.Commons;
using Domain.Entities;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.SyllabusModels;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Microsoft.AspNetCore.Session;
using Application.ViewModels.TrainingClassModels;
using Application.ViewModels.Location;
using Application.ViewModels.FeedbackModels;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.ApplicationViewModels;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.AtttendanceModels;
using Domain.Enums;
using Application.ViewModels.GradingModels;

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
            
            CreateMap<AttendanceDTO, Attendance>()
                .ForMember(a => a.Status, options => options.MapFrom(dto => dto.Status? nameof(AttendanceStatusEnums.Present): nameof(AttendanceStatusEnums.Absent)))
                .ForMember(a => a.UserId, options => options.MapFrom(dto => dto.UserId.Value))
                .ForMember(a => a.Id, options => options.MapFrom(dto => dto.AttendanceId))
                .ForMember(a => a.Date, options => options.MapFrom(dto => dto.Date))
                .ReverseMap();

            CreateMap<ResetPasswordDTO, User>()
                .ForMember(rp => rp.PasswordHash, opt => opt.MapFrom(src => src.NewPassword)).ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(desc => desc._Id, src => src.MapFrom(u => u.Id))
                .ReverseMap();

            CreateMap<SyllabusGeneralDTO, Syllabus>()
           .ForMember(ss => ss.SyllabusName, ss => ss.MapFrom(src => src.SyllabusName))
            .ForMember(ss => ss.Duration, ss => ss.MapFrom(src => src.Duration))
             .ForMember(ss => ss.CourseObjective, ss => ss.MapFrom(src => src.CourseObject))
               .ForMember(ss => ss.TechRequirements, ss => ss.MapFrom(src => src.TechRequirements))
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

            CreateMap<UnitDTO, Unit>()
                    .ForMember(uu => uu.UnitName, uu => uu.MapFrom(src => src.UnitName))
                    .ForMember(uu => uu.TotalTime, uu => uu.MapFrom(src => src.TotalTime))
                    .ForMember(uu => uu.Session, uu => uu.MapFrom(src => src.Session))
                    .ReverseMap();


            CreateMap<LectureDTO, Lecture>()
                    .ForMember(ll => ll.LectureName, ll => ll.MapFrom(src => src.LectureName))
                    .ForMember(ll => ll.OutputStandards, ll => ll.MapFrom(src => src.OutputStandards))
                    .ForMember(ll => ll.Duration, ll => ll.MapFrom(src => src.Duration))
                    .ForMember(ll => ll.DeliveryType, ll => ll.MapFrom(src => src.DeliveryType))
                    .ForMember(ll => ll.Status, ll => ll.MapFrom(src => src.Status))
                    .ReverseMap();


            CreateMap<User, LoginWithEmailDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            //map training class
            CreateMap<CreateTrainingClassDTO, TrainingClass>();
            CreateMap<TrainingClass, TrainingClassViewModel>()
                .ForMember(x => x._Id, src => src.MapFrom(x => x.Id))
                .ForMember(x=>x.LocationName, src =>src.MapFrom(x=>x.Location.LocationName));

            //map location
            CreateMap<CreateLocationDTO, Location>();
            CreateMap<Location, LocationDTO>()
                .ForMember(x => x._Id, src => src.MapFrom(x => x.Id));
            CreateMap<TrainingProgram, TrainingProgramViewModel>()
                .ForMember(dest => dest.TrainingProgramId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreationDate)).ReverseMap();

  
            CreateMap<CreateTrainingProgramDTO, TrainingProgram>().ReverseMap();
            CreateMap<UpdateTrainingProgramDTO, TrainingProgram>().ReverseMap();

            CreateMap<Feedback, FeedbackModel>().ReverseMap();
            CreateMap<Feedback, FeedbackVM>().ReverseMap();

            CreateMap<TrainingProgram, TrainingProgramViewModel>()
                .ForMember(dest => dest.TrainingProgramId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreationDate)).ReverseMap();


            CreateMap<CreateTrainingProgramDTO, TrainingProgram>().ReverseMap();
            CreateMap<UpdateTrainingProgramDTO, TrainingProgram>().ReverseMap();

            CreateMap<Applications, ApplicationDTO>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.Appoved, opt => opt.MapFrom(src => src.Approved))
                .ForMember(dest => dest.AbsentDateRequested, opt => opt.MapFrom(src => src.AbsentDateRequested)).ReverseMap();

            CreateMap<Grading, GradingModel>().ReverseMap();
        }
    }
}
