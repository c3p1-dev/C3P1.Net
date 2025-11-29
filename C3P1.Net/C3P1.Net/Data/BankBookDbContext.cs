using C3P1.Net.Client.Data.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data
{
    public class BankBookDbContext(DbContextOptions<BankBookDbContext> options) : DbContext(options)
    {
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    }
}
