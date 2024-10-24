using SaleNotifier.TelegramBot.Services.Interfaces;
using SalesNotifier.Persistence;
using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Services;

public class SaleService(ApplicationDbContext context) : ISaleService
{
    public async Task CreateSale(string title, string description, string maleAssortment, string femaleAssortment, string contactLink)
    {
        var sale = new Sale
        {
            Title = title,
            Description = description,
            MaleAssortment = maleAssortment,
            FemaleAssortment = femaleAssortment,
            InstagramPage = contactLink
        };
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();
    }
}