namespace SaleNotifier.TelegramBot.Services.Interfaces;

public interface IMenuService
{
    Dictionary<string,  List<string>> Commands { get; set; }
    
    public List<string>? GetCommandToExecute(string keyMessage);
}