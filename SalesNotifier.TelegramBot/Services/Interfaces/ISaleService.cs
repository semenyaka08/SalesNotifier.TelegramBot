using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Services.Interfaces;

public interface ISaleService
{
    Task CreateSale(string title, string description, string maleAssortment, string femaleAssortment, string contactLink);
}