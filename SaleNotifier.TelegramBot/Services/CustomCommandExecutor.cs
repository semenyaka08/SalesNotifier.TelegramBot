using CommandWorkflows.Infrastructure.CommandExecutor;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using SaleNotifier.TelegramBot.Communication;
using SaleNotifier.TelegramBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace SaleNotifier.TelegramBot.Services;

public class CustomCommandExecutor : CommandExecutor<long>, ICustomCommandExecutor
{
    public CustomCommandExecutor(ICommandHistoryService<long> commandHistoryService, ICommandResolver commandResolver, ILogger<CommandExecutor<long>> logger) : base(commandHistoryService, commandResolver, logger)
    { }


    public async Task<Response> ExecuteCommandAsync(Update update, long userId)
    {
        var request = new Request
        {
            Message = update.Message?.Text ?? string.Empty
        };
        
        return await base.ExecuteCommandAsync<Request, Response>(request, userId);
    }
}