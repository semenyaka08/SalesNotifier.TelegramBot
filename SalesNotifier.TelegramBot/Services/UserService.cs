using Microsoft.EntityFrameworkCore;
using SaleNotifier.TelegramBot.Services.Interfaces;
using SalesNotifier.Persistence;
using SalesNotifier.Persistence.Entities;

namespace SaleNotifier.TelegramBot.Services;

public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<bool> IsForbidden(long userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(z=>z.Id == userId);

        return user is not null && user.UserType == UserType.Admin;
    }
}