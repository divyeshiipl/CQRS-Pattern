using Application.Interfaces;
using Domain.Common;
using Infrastructure.Services;
using Infrastructure.Services.Common;
using Microsoft.OpenApi.Models;
using Serilog;
using Web.Handlers;

namespace Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddScoped<IUser, CurrentUser>();
        builder.Services.AddSingleton<ILogService, LogService>();
        builder.Services.AddSingleton<IErrorResponseService, ErrorResponseService>();


        builder.Services.AddHttpClient<IApiClientService, ApiClientService>();

        builder.Services.AddHttpContextAccessor();
        //builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        builder.ConfigureExceptionHandling();
        builder.ConfigureApiBehavior();
        builder.ConfigureSwagger();
        builder.Services.AddControllers();
        builder.ConfigureLogging();
        //builder.ConfigureSystemSettings();
    }

    /// <summary>
    /// Configures global exception handling.
    /// </summary>
    private static void ConfigureExceptionHandling(this IHostApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
    }

    /// <summary>
    /// Customizes default API behavior.
    /// </summary>
    private static void ConfigureApiBehavior(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
    }

    /// <summary>
    /// Configures Swagger with JWT authentication support.
    /// </summary>
    private static void ConfigureSwagger(this IHostApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NAJM Vehicle Insureance Inquiry API", Version = "v1" });

            // Define the security scheme
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            // Apply security to all operations
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new List<string>()
                }
            });
        });
    }


    /// <summary>
    /// Configures Serilog logging using external configuration files.
    /// </summary>
    private static void ConfigureLogging(this IHostApplicationBuilder builder)
    {
        IConfigurationManager configuration = builder.Configuration;
        configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("serilog.json")
                     .AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                     .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
    }

    private static void ConfigureSystemSettings(this IHostApplicationBuilder builder)
    {
        IConfigurationManager configuration = builder.Configuration;

        configuration.AddCommonsDbConfiguration("ApplicationConnection");

        builder.Services.Configure<CAMSAPIConfiguration>(options =>
        {
            configuration.GetSection(CAMSAPIConfiguration.Name).Bind(options);
        });

        builder.Services.Configure<LDAPIConfiguration>(options =>
        {
            configuration.GetSection(LDAPIConfiguration.Name).Bind(options);
        });
    }
}