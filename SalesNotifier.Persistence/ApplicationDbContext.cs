using Microsoft.EntityFrameworkCore;
using SalesNotifier.Persistence.Entities;

namespace SalesNotifier.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<Sale> Sales { get; set; }
    
    public DbSet<User> Users { get; set; }
}