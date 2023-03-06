using Application.Commons;
using Application.Utils;
using Hangfire;
using Infrastructures;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebAPI;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// parse the configuration in appsettings
var configuration = builder.Configuration.Get<AppConfiguration>();

// Them CORS cho tat ca moi nguoi deu xai duoc apis
builder.Services.AddCors(options
    => options.AddDefaultPolicy(policy
        => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddInfrastructuresService(configuration!.DatabaseConnection);
builder.Services.AddWebAPIService(configuration!.JWTSecretKey);

/*
    register with singleton life time
    now we can use dependency injection for AppConfiguration
*/
builder.Services.AddSingleton(configuration);
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Dang ki hangfire de thuc hien cron job
builder.Services.AddHangfire(config => config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseInMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();

// Bat Cors
app.UseCors();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// add custom middlewares
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
// App health check at root/healthchecks
app.MapHealthChecks("/healthchecks");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// hangfire host dashboard at "/dashboard"
app.MapHangfireDashboard("/dashboard");

// call hangfire
await app.StartAsync();
RecurringJob.AddOrUpdate<ApplicationCronJob>(util => util.CheckAttendancesEveryDay(),
    "0 0 22 ? * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
await app.WaitForShutdownAsync();

app.Run();
