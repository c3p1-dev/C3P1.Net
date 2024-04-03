using C3P1.Net.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        // Set Tables in database
        public DbSet<TodoItem> Tasklist => Set<TodoItem>();
        public DbSet<Cat> Cats => Set<Cat>();
        public DbSet<CatEntry> CatEntries => Set<CatEntry>();
        public DbSet<AppParam> UsersAppParam => Set<AppParam>();
    }
}
