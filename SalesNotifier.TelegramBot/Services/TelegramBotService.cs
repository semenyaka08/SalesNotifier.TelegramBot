using SaleNotifier.TelegramBot.Communication;
using SaleNotifier.TelegramBot.Services.Interfaces;
using SalesNotifier.Persistence.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SaleNotifier.TelegramBot.Services;

public class TelegramBotService : ITelegramBotService
{
    private TelegramBotClient _telegramBotClient;
    private readonly Settings _settings;
    private readonly ILogger<TelegramBotService> _logger;
    private readonly ICustomCommandExecutor _commandExecutor;
    private readonly IMenuService _menuService;

    public TelegramBotService(Settings settings, ILogger<TelegramBotService> logger, ICustomCommandExecutor commandExecutor, IMenuService menuService)
    {
        _settings = settings;
        _logger = logger;
        _commandExecutor = commandExecutor;
        _menuService = menuService;
        _telegramBotClient = new TelegramBotClient(settings.TelegramBotToken);
    }

    public void StartBotReceive(CancellationToken token = default)
    {
        var receiverOptions = new ReceiverOptions();
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
        long userId;
        string message;
        Response response;
        
        switch (update.Type)
        {
            case UpdateType.Message:
                userId = update.Message!.From.Id;
                message = update.Message!.Text;
                break;
            case UpdateType.CallbackQuery:
                userId = update.CallbackQuery!.From.Id;
                message = update.CallbackQuery!.Data;
                break;
            default:
                return;
        }
        List<string> commandsToExecute = _menuService.GetCommandToExecute(message!);
        
        if (commandsToExecute == null)
        {
            try
            {
                response = await _commandExecutor.ExecuteCommandAsync(new Request { Message = message! }, userId);
                await SendFormattedMessageAsync(response, userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                await SendFormattedMessageAsync(new Response
                {
                    Format = Format.PlainText,
                    Message = "Невідома команда, спробуйте ще раз",
                    ResponseType = ResponseType.CommandType
                }, userId, cancellationToken);
            }
        }
        else
        {
            foreach (var command in commandsToExecute)
            {
                response = await _commandExecutor.ExecuteCommandAsync(new Request { Message = command }, userId);
                await SendFormattedMessageAsync(response, userId, cancellationToken);
            }
        }
    }

    private async Task SendFormattedMessageAsync(Response response, long chatId, CancellationToken cancellationToken)
    {
        switch (response.Format)
        {
            case Format.PlainText:
                await _telegramBotClient.SendTextMessageAsync(chatId, response.Message, cancellationToken: cancellationToken);
                break;

            case Format.TextWithKeyboard:
                var replyMarkup = new ReplyKeyboardMarkup(
                    response.Buttons.Select(b => new KeyboardButton(b.Text)).ToArray())
                {
                    ResizeKeyboard = true
                };
                await _telegramBotClient.SendTextMessageAsync(chatId, response.Message, replyMarkup: replyMarkup, cancellationToken: cancellationToken);
                break;
            case Format.InlineKeyboard:
                var inlineMarkup = new InlineKeyboardMarkup(
                    response.Buttons.Select(button => InlineKeyboardButton.WithCallbackData(button.Text, button.CallbackData)).ToArray()
                );
                await _telegramBotClient.SendTextMessageAsync(chatId, response.Message, replyMarkup: inlineMarkup, cancellationToken: cancellationToken);
                break;
            default:
                await _telegramBotClient.SendTextMessageAsync(chatId, "Not recognized type of response", cancellationToken: cancellationToken);
                break;
        }
    }

    public async Task SetInitialMenuAsync()
    {
        var commands = new List<BotCommand>
        {
            new BotCommand { Command = "/addsale", Description = "create a new sale" }
        };

        await _telegramBotClient.SetMyCommandsAsync(commands);
    }
}