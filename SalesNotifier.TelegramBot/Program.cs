using SaleNotifier.TelegramBot;
using SaleNotifier.TelegramBot.Services.Interfaces;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    Log.Information($"Environment: {environmentVariable}");
    
    var host = CreateHostBuilder(args).Build();
    
    using (var scope = host.Services.CreateScope())
    {
        var telegramService = scope.ServiceProvider.GetRequiredService<ITelegramBotService>();
        
        // Установка команд (или ClearBotCommandsAsync для очистки)
        await telegramService.SetInitialMenuAsync(); // или ClearBotCommandsAsync()
    }
    
    host.StartReceive();

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
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
    
    return Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
        )
        .ConfigureServices(services => services.ConfigureServices(config));
}