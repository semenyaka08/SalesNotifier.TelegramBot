using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddDescriptionWorkflow(PersistentAdminData persistentAdminData) : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData.Sale.Description = request.Message;
        return Task.FromResult(new Response
        {
            Message = "Додайте посилання на чоловічий асортимент",
            Format = Format.PlainText
        });
    }
}