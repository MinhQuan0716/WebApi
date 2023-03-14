using Application.Commons;
using Application.Utils;
using Hangfire;
using Hangfire.Logging;
using Infrastructures;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;
using WebAPI;
using WebAPI.Middlewares;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    // parse the configuration in appsettings
    var configuration = builder.Configuration.Get<AppConfiguration>();
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
    // Them CORS cho tat ca moi nguoi deu xai duoc apis
    builder.Services.AddCors(options
        => options.AddDefaultPolicy(policy
            => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

    builder.Services.AddInfrastructuresService(configuration!.DatabaseConnection);
    builder.Services.AddWebAPIService(configuration!.JWTSecretKey);


    //register with singleton life time
    //now we can use dependency injection for AppConfiguration

// add file provider
    var physicalProvider = builder.Environment.ContentRootFileProvider;
    var manifestEmbeddedProvider =
        new ManifestEmbeddedFileProvider(typeof(Program).Assembly);
    var compositeProvider =
        new CompositeFileProvider(physicalProvider, manifestEmbeddedProvider);

    builder.Services.AddSingleton<IFileProvider>(compositeProvider);

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
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
        });
    });

        // Avoid MultiPartBodyLength error - UploadFile Controller
        builder.Services.Configure<FormOptions>(o =>
    {
        o.ValueLengthLimit = int.MaxValue;
        o.MultipartBodyLengthLimit = int.MaxValue;
        o.MemoryBufferThreshold = int.MaxValue;
    });

        // Dang ki hangfire de thuc hien cron job
        builder.Services.AddHangfire(config => config
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage());
        builder.Services.AddHangfireServer();

        var app = builder.Build();
        // Modify log message of serilog 
        app.UseSerilogRequestLogging(configure =>
        {
            configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
        });
        // Bat Cors
        app.UseCors();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();
        // add custom middlewares
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<PerformanceMiddleware>();
        app.UseMiddleware<StaticFileMiddleware>();

        // App health check at root/healthchecks
        app.MapHealthChecks("/healthchecks");
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // add custom middlewares




        app.UseMiddleware<GlobalExceptionMiddlewareV2>();

        app.UseMiddleware<PerformanceMiddleware>();
        // App health check at root/healthchecks 
        app.MapHealthChecks("/healthchecks");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseStaticFiles();



        // hangfire host dashboard at "/dashboard"
        app.MapHangfireDashboard("/dashboard");

        // call hangfire
        await app.StartAsync();
        RecurringJob.AddOrUpdate<ApplicationCronJob>(util => util.CheckAttendancesEveryDay(),
            "0 0 22 ? * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        await app.WaitForShutdownAsync();

        app.Run();



    }
catch (Exception ex)
{
    Log.Fatal(ex, "Host Terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}



