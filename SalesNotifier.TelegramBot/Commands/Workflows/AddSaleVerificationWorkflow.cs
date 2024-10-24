using CommandWorkflows.Infrastructure.Abstraction;
using SaleNotifier.TelegramBot.Communication;
using SaleNotifier.TelegramBot.Services.Interfaces;
using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Commands.Workflows;

public class AddSaleVerificationWorkflow(PersistentAdminData persistentAdminData, ISaleService saleService) : IWorkflow<Request, Response>
{
    public async Task<Response> ExecuteAsync(Request request)
    {
        var sale = persistentAdminData.Sale;
        await saleService.CreateSale(sale.Title, sale.Description, sale.MaleAssortment, sale.FemaleAssortment, sale.InstagramPage);

        Response response = new Response
        {
            Message = $"{persistentAdminData.Sale.Title} був успішно створений!"
        };
        
        persistentAdminData.Sale = new Sale();

        return response;
    }
}