using C3P1.Net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Admin
{
    public class UsersAdminService : IUsersAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersAdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            return (await _userManager.DeleteAsync(user)).Succeeded;
        }
        public async Task<List<ApplicationUser>> GetAdminUsersAsync()
        {
            return (await _userManager.GetUsersInRoleAsync("Admin")).ToList();
        }
        public async Task<List<ApplicationUser>> GetRegularUsersAsync()
        {
            List<ApplicationUser> regularusers = await GetUsersAsync();
            List<ApplicationUser> admins = await GetAdminUsersAsync();

            foreach (ApplicationUser user in admins)
            {
                regularusers.Remove(user);
            }

            return regularusers;
        }
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<bool> MakeAdminAsync(ApplicationUser user)
        {
            return (await _userManager.AddToRoleAsync(user, "Admin")).Succeeded;
        }
        public async Task<bool> MakeRegularAsync(ApplicationUser user)
        {
            return (await _userManager.RemoveFromRoleAsync(user, "Admin")).Succeeded;
        }

        public async Task<bool> IsAdminAsync(ApplicationUser user)
        {
            var result = (await _userManager.GetRolesAsync(user)).Where(x => x == "Admin").FirstOrDefault();

            if (result == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
