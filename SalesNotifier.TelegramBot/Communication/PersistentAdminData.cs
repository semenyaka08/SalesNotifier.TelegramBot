using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Communication;

public class PersistentAdminData
{
    public Sale Sale { get; set; } = new();
}