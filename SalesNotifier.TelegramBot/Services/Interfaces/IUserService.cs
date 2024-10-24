using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Services.Interfaces;

public interface IUserService
{
    Task<bool> IsForbidden(long userId);

    Task AddUser(User user);
}