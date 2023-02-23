using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.Services;
using Application.Utils;
using Infrastructures.Mappers;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Infrastructures
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddScoped<ISendMailHelper, SendMailHelper>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISyllabusRepository, SyllabusRepository>();
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDetailUnitLectureRepository, DetailUnitLectureRepository>();
            services.AddScoped<ITrainingClassRepository, TraingClassRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddSingleton<ICurrentTime, CurrentTime>();
  
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISyllabusService, SyllabusService>();
            services.AddSingleton<ExternalAuthUtils>();

            services.AddScoped<ITrainingClassRepository, TraingClassRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();
            services.AddScoped<ITrainingProgramService, TrainingProgramService>();
            services.AddScoped<IDetailTrainingProgramSyllabusRepository, DetailTrainingProgramSyllabusRepository>();


            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // ATTENTION: if you do migration please check file README.md
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection).EnableSensitiveDataLogging());

            // this configuration just use in-memory for fast develop
            //services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("test"));

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);            

            return services;
        }
    }
}
