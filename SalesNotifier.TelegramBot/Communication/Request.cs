using CommandWorkflows.Infrastructure.Abstraction;

namespace SaleNotifier.TelegramBot.Communication;

public class Request : IRequest
{
    public string Message { get; set; } = string.Empty;
}