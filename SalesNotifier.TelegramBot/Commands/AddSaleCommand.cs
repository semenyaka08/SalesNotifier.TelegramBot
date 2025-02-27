using CommandWorkflows.Infrastructure.Abstraction.Commands;
using SaleNotifier.TelegramBot.Communication;

namespace SaleNotifier.TelegramBot.Commands;

public class AddSaleCommand : CommandAbstract<Request, Response>
{
    public override Task<Response> ExecuteAsync(Request request)
    {
        return Task.FromResult(new Response
        {
            Message = "Додайте зоголовок до сейлу",
            Format = Format.PlainText,
            ResponseType = ResponseType.CommandType
        });
    }
}