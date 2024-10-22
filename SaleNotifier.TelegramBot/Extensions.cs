using SaleNotifier.TelegramBot.Services;
using SaleNotifier.TelegramBot.Services.Interfaces;
using Serilog;

namespace SaleNotifier.TelegramBot;

public static class Extensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddScoped<Settings>(_ => config.Get<Settings>()!);
        services.AddScoped<ITelegramBotService, TelegramBotService>();
        services.AddScoped<ICustomCommandExecutor, CustomCommandExecutor>();
    }
    
    public static void StartReceive(this IHost host)
    {
        Log.Information("Try to get TelegramBotService instance...");
        var service = host.Services.GetRequiredService<ITelegramBotService>();
        Log.Information("Get TelegramBotService instance completed");
        Log.Information("Start Receiving");
        var cancellationToken = new CancellationToken();
        service.StartBotReceive(cancellationToken);
        host.WaitForShutdown();
    }

}