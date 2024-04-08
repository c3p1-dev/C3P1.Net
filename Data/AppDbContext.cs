using C3P1.Net.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        // Set Tables in database
        public DbSet<TodoItem> Tasklist => Set<TodoItem>();
        public DbSet<Cat> Cats => Set<Cat>();
        public DbSet<CatEntry> CatEntries => Set<CatEntry>();
        public DbSet<AppConfig> UserAppConfig => Set<AppConfig>();
    }
}
