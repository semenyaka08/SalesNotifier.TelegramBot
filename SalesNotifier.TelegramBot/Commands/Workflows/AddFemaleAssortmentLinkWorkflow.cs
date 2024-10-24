using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddFemaleAssortmentLinkWorkflow(PersistentAdminData persistentAdminData) : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData.Sale.FemaleAssortment = request.Message;
        return Task.FromResult(new Response
        {
            Message = "Додайте посилання для зв'язку з вами",
            Format = Format.PlainText
        });
    }
}