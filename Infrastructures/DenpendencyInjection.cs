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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<ICurrentTime, CurrentTime>();
            

            services.AddScoped<ISyllabusService, SyllabusService>();
   
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ICurrentTime, CurrentTime>();
            services.AddScoped<ISyllabusRepository, SyllabusRepository>();
            //Add Policy
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanUpdateOrder", policy => policy.RequireClaim("SyllabusPermission", "View"));
            //});

            //services.AddSingleton<IAuthorizationHandler, SyllabusPermissionAuthorizationHandler>();
            // ATTENTION: if you do migration please check file README.md
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));

            // this configuration just use in-memory for fast develop
            //services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("test"));

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

            return services;
        }
    }
}
