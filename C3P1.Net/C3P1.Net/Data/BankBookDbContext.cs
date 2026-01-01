using C3P1.Net.Shared.Data.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data
{
    public class BankBookDbContext(DbContextOptions<BankBookDbContext> options) : DbContext(options)
    {
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    }
}
