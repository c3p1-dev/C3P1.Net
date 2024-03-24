using C3P1.Net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data
{
    public class SeedDatabase
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await EnsureTestAdminAsync(userManager);
            await EnsureTestUserAsync(userManager);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Create Admin Role
            bool alreadyExists = await roleManager.RoleExistsAsync("Admin");
            if (!alreadyExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create User Role
            alreadyExists = await roleManager.RoleExistsAsync("User");
            if (!alreadyExists)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser? newAdmin = await userManager.Users
                    .Where(x => x.UserName == "webmaster@c3p1.net")
                    .FirstOrDefaultAsync();

            if (newAdmin is null)
            {
                newAdmin = new ApplicationUser
                {
                    UserName = "webmaster@c3p1.net",
                    Email = "webmaster@c3p1.net",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newAdmin, "W3m1@010170");
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }

        private static async Task EnsureTestUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser? newUser = await userManager.Users
                .Where(x => x.UserName == "user@c3p1.net")
                .FirstOrDefaultAsync();

            if (newUser is null)
            {
                newUser = new ApplicationUser
                {
                    UserName = "user@c3p1.net",
                    Email = "user@c3p1.net",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(newUser, "U3s1@010170");
                await userManager.AddToRoleAsync(newUser, "User");
            }
        }
    }
}
