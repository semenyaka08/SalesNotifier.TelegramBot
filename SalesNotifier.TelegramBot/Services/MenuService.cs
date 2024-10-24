using SaleNotifier.TelegramBot.Commands;
using SaleNotifier.TelegramBot.Services.Interfaces;

namespace SaleNotifier.TelegramBot.Services;

public class MenuService : IMenuService
{
    public MenuService()
    {
        Commands = new Dictionary<string, List<string>>
        {
            {CommandConstants.AddSaleCommand, [CommandConstants.AddSaleCommand] },
            { CommandConstants.CancelSaleCreation, [CommandConstants.ExitCommand, CommandConstants.CancelSaleCreation] },
            { CommandConstants.CancelSaleRecreation, [CommandConstants.CancelSaleRecreation]}
        };
    }

    public Dictionary<string, List<string>> Commands { get; set; }
    public List<string>? GetCommandToExecute(string keyMessage)
    {
        return Commands.TryGetValue(keyMessage, out List<string>? commands) ? commands : null;
    }
}