using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using SaleNotifier.TelegramBot.Communication;


namespace SaleNotifier.TelegramBot.Commands;

public class ExitCommand : IPermanentExitCommand<Request, Response>
{
    public Task<Response> ExecuteAsync(Request request)
    {
        return Task.FromResult(new Response
        {
            Format = Format.PlainText,
            Message = "ок\ud83d\udc4d"
        });
    }
    
    public Queue<IWorkflow<Request, Response>> Workflows { get; set; }
}