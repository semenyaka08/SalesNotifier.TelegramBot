using System.Text;
using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;
using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddContactLinkWorkflow(PersistentAdminData persistentAdminData) : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData.Sale.InstagramPage = request.Message;
        
        var (text, buttons) = GenerateResponseTextAndButtons(persistentAdminData.Sale);
        
        return Task.FromResult(new Response
        {
            Buttons = buttons,
            Message = text,
            Format = Format.InlineKeyboard
        });
    }

    private (string responseText, List<ResponseButton> buttons) GenerateResponseTextAndButtons(Sale sale)
    {
        StringBuilder stringBuilder = new StringBuilder("Перевірте правильність введених даних");

        stringBuilder.AppendLine($"\n\n1.Title: {persistentAdminData.Sale.Title}");
        stringBuilder.AppendLine($"2.Description: {persistentAdminData.Sale.Description}");
        stringBuilder.AppendLine($"3.MaleAssortment: {persistentAdminData.Sale.MaleAssortment}");
        stringBuilder.AppendLine($"4.FemaleAssortment: {persistentAdminData.Sale.FemaleAssortment}");
        stringBuilder.AppendLine($"5.ContactLink: {persistentAdminData.Sale.InstagramPage}");
        
        stringBuilder.AppendLine("\nВсе правильно? Натисніть \"так\" або \"ні\"");

        var buttons = new List<ResponseButton>{
            new() { Text = "Так", CallbackData = "ConfirmSaleCreation" }, 
            new() { Text = "Ні", CallbackData = "CancelSaleCreation" }
        };

        return (stringBuilder.ToString(), buttons);
    }
}