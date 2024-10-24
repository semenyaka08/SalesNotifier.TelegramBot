using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddTitleWorkflow(PersistentAdminData persistentAdminData) : IWorkflow<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        persistentAdminData.Sale.Title = request.Message;
        return Task.FromResult(new Response
        {
            Message = "Додайте опис до сейлу",
            Format = Format.PlainText
        });
    }
}