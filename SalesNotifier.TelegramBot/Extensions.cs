using CommandWorkflows.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SaleNotifier.TelegramBot.Commands;
using SaleNotifier.TelegramBot.Commands.Workflows;
using SaleNotifier.TelegramBot.Communication;
using SaleNotifier.TelegramBot.Services;
using SaleNotifier.TelegramBot.Services.Interfaces;
using SalesNotifier.Persistence;
using Serilog;

namespace SaleNotifier.TelegramBot;

public static class Extensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfigurationRoot config)
    {
        services.AddCommandRegistry<long>(ServiceLifetime.Scoped);
        services.AddScoped<Settings>(_ => config.Get<Settings>()!);
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITelegramBotService, TelegramBotService>();
        services.AddScoped<ICustomCommandExecutor, CustomCommandExecutor>();
        services.AddScoped<PersistentAdminData>();
        services.AddScoped<IMenuService, MenuService>();
        services.RegisterCommand<RecreateSaleCommand>(CommandConstants.CancelSaleCreation, ServiceLifetime.Scoped);
        services.RegisterCommand<ExitCommand>(CommandConstants.ExitCommand, ServiceLifetime.Scoped);
        services.RegisterCommand<CancelSaleRecreationCommand>(CommandConstants.CancelSaleRecreation, ServiceLifetime.Scoped);
        services.RegisterCommand<AddSaleCommand>(CommandConstants.AddSaleCommand, ServiceLifetime.Scoped)
            .RegisterWorkflow<AddTitleWorkflow>()
            .RegisterWorkflow<AddDescriptionWorkflow>()
            .RegisterWorkflow<AddMaleAssortmentLinkWorkflow>()
            .RegisterWorkflow<AddFemaleAssortmentLinkWorkflow>()
            .RegisterWorkflow<AddContactLinkWorkflow>()
            .RegisterWorkflow<AddSaleVerificationWorkflow>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });
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