using C3P1.Net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Admin
{
    public class UserAdminService : IUserAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserAdminService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> DeleteUserAsync(AppUser user)
        {
            return (await _userManager.DeleteAsync(user)).Succeeded;
        }
        public async Task<List<AppUser>> GetAdminUsersAsync()
        {
            return (await _userManager.GetUsersInRoleAsync("Admin")).ToList();
        }
        public async Task<List<AppUser>> GetRegularUsersAsync()
        {
            List<AppUser> regularusers = await GetUsersAsync();
            List<AppUser> admins = await GetAdminUsersAsync();

            foreach (AppUser user in admins)
            {
                regularusers.Remove(user);
            }

            return regularusers;
        }
        public async Task<List<AppUser>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<bool> MakeAdminAsync(AppUser user)
        {
            return (await _userManager.AddToRoleAsync(user, "Admin")).Succeeded;
        }
        public async Task<bool> MakeRegularAsync(AppUser user)
        {
            return (await _userManager.RemoveFromRoleAsync(user, "Admin")).Succeeded;
        }

        public async Task<bool> IsAdminAsync(AppUser user)
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
