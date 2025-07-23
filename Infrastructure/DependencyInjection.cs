namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddDbInterceptor();

        var environment = builder.Environment;
        builder.AddMiniProfiler(environment);

        //builder.AddAppDbContext(environment);

        builder.AddAppServices();
    }

    private static void AddAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddSingleton(TimeProvider.System);
    }

    private static void AddDbInterceptor(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
    }

    private static void AddMiniProfiler(this IHostApplicationBuilder builder, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            builder.Services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler"; // URL to access profiler data
                options.TrackConnectionOpenClose = true; // Tracks DB connection lifecycle
            }).AddEntityFramework();
        }
    }

    //private static void AddAppDbContext(this IHostApplicationBuilder builder, IHostEnvironment environment)
    //{
    //    var connectionString = builder.Configuration.GetConnectionString("ApplicationConnection");
    //    Guard.Against.Null(connectionString, message: "Connection string 'NajmVehicleInsurance Db' not found.");

    //    builder.Services.AddDbContext<DbContext>((sp, options) =>
    //    {
    //        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
    //        options.UseSqlServer(connectionString);
    //        if (environment.IsDevelopment())
    //        {
    //            options.EnableSensitiveDataLogging();
    //            options.EnableDetailedErrors();
    //        }
    //    });

    //}
}
