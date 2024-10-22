using SaleNotifier.TelegramBot;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");
try
{
    var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    Log.Information($"Environment: {environmentVariable}");
    CreateHostBuilder(args).Build().StartReceive();
    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{environmentVariable}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
    
    return Host.CreateDefaultBuilder(args).
        UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
        )
        .ConfigureServices(services => services.ConfigureServices(config));
}