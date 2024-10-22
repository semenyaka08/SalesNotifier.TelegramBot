using SaleNotifier.TelegramBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SaleNotifier.TelegramBot.Services;

public class TelegramBotService : ITelegramBotService
{
    private TelegramBotClient _telegramBotClient;
    private readonly Settings _settings;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly ICustomCommandExecutor _commandExecutor;

    public TelegramBotService(Settings settings, ILogger<TelegramBotService> logger, ICustomCommandExecutor commandExecutor)
    {
        _settings = settings;
        _logger = logger;
        _commandExecutor = commandExecutor;
        _telegramBotClient = new TelegramBotClient(settings.TelegramBotToken);
    }

    public void StartBotReceive(CancellationToken token = default)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };
        _telegramBotClient = new TelegramBotClient(_settings.TelegramBotToken);
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: token);

    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(errorMessage);
        return Task.CompletedTask;

    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            await HandleUpdateBody(update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("{@Exception}", ex);
        }

    }

    private async Task HandleUpdateBody(Update update, CancellationToken cancellationToken)
    {
        long userId = update.Message!.From.Id;
        
        var result = await _commandExecutor.ExecuteCommandAsync(update, userId);
    }
}