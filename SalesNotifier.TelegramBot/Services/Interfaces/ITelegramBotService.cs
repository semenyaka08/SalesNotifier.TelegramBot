namespace SaleNotifier.TelegramBot.Services.Interfaces;

public interface ITelegramBotService
{
    public void StartBotReceive(CancellationToken token);
}