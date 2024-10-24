using CommandWorkflows.Infrastructure.Abstraction.Commands;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands;

public class RecreateSaleCommand(PersistentAdminData persistentAdminData) : CommandAbstract<Request, Response>
{
    public override Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData = new PersistentAdminData();
        
        var result = GenerateResponseTextAndButtons();
        
        return Task.FromResult(new Response
        {
            Message = result.responseText,
            Format = Format.InlineKeyboard,
            Buttons = result.buttons,
            ResponseType = ResponseType.CommandType
        });
    }
    
    private (string responseText, List<ResponseButton> buttons) GenerateResponseTextAndButtons()
    {
        var buttons = new List<ResponseButton>{
            new() { Text = "Так", CallbackData = CommandConstants.AddSaleCommand }, 
            new() { Text = "Ні", CallbackData = CommandConstants.CancelSaleRecreation }
        };

        return ("Бажаєте перестворити сейл?", buttons);
    }
}