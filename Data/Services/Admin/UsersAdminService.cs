using C3P1.Net.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Data.Services.Admin
{
    public class UsersAdminService : IUsersAdminService
    {
        private readonly UserManager<C3P1User> _userManager;

        public UsersAdminService(UserManager<C3P1User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> DeleteUserAsync(C3P1User user)
        {
            return (await _userManager.DeleteAsync(user)).Succeeded;
        }
        public async Task<List<C3P1User>> GetAdminUsersAsync()
        {
            return (await _userManager.GetUsersInRoleAsync("Admin")).ToList();
        }
        public async Task<List<C3P1User>> GetRegularUsersAsync()
        {
            List<C3P1User> regularusers = await GetUsersAsync();
            List<C3P1User> admins = await GetAdminUsersAsync();

            foreach (C3P1User user in admins)
            {
                regularusers.Remove(user);
            }

            return regularusers;
        }
        public async Task<List<C3P1User>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<bool> MakeAdminAsync(C3P1User user)
        {
            return (await _userManager.AddToRoleAsync(user, "Admin")).Succeeded;
        }
        public async Task<bool> MakeRegularAsync(C3P1User user)
        {
            return (await _userManager.RemoveFromRoleAsync(user, "Admin")).Succeeded;
        }
    }
}
