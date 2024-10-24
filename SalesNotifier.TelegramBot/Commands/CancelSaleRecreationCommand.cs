using CommandWorkflows.Infrastructure.Abstraction.Commands;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands;

public class CancelSaleRecreationCommand : CommandAbstract<Request, Response>
{
    public override Task<Response> ExecuteAsync(Request request)
    {
        return Task.FromResult(new Response
        {
            Message = "Створення нового сейлу було відмінено",
            Format = Format.PlainText,
            ResponseType = ResponseType.CommandType
        });
    }
}