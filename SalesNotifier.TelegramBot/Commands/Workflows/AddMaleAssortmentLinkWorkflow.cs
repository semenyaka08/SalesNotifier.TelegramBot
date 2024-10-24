using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddMaleAssortmentLinkWorkflow(PersistentAdminData persistentAdminData) : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData.Sale.MaleAssortment = request.Message;
        return Task.FromResult(new Response
        {
            Message = "Додайте посилання на жіночий асортимент",
            Format = Format.PlainText
        });
    }
}