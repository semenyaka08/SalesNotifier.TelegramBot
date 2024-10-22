using SaleNotifier.TelegramBot.Communication;
using Telegram.Bot.Types;

namespace SaleNotifier.TelegramBot.Services.Interfaces;

public interface ICustomCommandExecutor
{
    Task<Response> ExecuteCommandAsync(Update update, long userId);
}