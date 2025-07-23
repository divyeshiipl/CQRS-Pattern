var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

var lAppSettingsEnvironment = configuration.GetValue<string>("ENVIRONMENT");
builder.Environment.EnvironmentName = lAppSettingsEnvironment ?? builder.Environment.EnvironmentName;

var serilogConfig = configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("serilog.json")
    .AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(serilogConfig).Enrich.With(new SeriLogCustomPropertyEnricher());
});

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

string? baseAddress = configuration.GetValue<string>("AppSettings:BaseAddress");
string? audience = configuration.GetValue<string>("AppSettings:Audience");

builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer("Bearer", config =>
   {
       config.Authority = baseAddress;
       config.Audience = audience;

       config.Events = new JwtBearerEvents
       {
           OnChallenge = async context =>
           {
               // Prevent default response
               context.HandleResponse();

               var errorService = context.HttpContext.RequestServices
                   .GetRequiredService<IErrorResponseService>();

               await errorService.HandleUnauthorizedAccessException(
                   context.HttpContext,
                   new UnauthorizedAccessException()
               );
           },
           OnForbidden = async context =>
           {
               var errorService = context.HttpContext.RequestServices
                   .GetRequiredService<IErrorResponseService>();

               await errorService.HandleForbiddenAccessException(
                   context.HttpContext,
                   new Exception()
               );
           }
       };
   });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISeriLogAppService, SeriLogAppService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuditLogMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

app.MapControllers(); //Enable controller routes

await app.RunAsync();