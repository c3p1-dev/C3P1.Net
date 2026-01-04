using C3P1.Net.Data;
using C3P1.Net.Shared.Data;
using C3P1.Net.Shared.Services.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Admin
{
    public class UserServerService(AppDbContext context, UserManager<AppUser> userManager) : IUserService
    {

        private readonly AppDbContext _context = context;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<List<AppUserDto>> GetUsersAsync()
        {
            var result = await _userManager.Users.ToListAsync();
            List<AppUserDto> users = [];
            foreach (var item in result)
                users.Add(ToDto(item));

            return users;
        }

        public async Task<List<AppUserDto>> GetUsersInRoleAsync(string role)
        {
            List<AppUserDto> result = [];
            foreach (var user in _userManager.Users)
            {
                bool isInRole = await _userManager.IsInRoleAsync(user, role);
                if (isInRole)
                    result.Add(ToDto(user));
            }

            return result;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            List<string> result = [];
            foreach (var role in roles)
                result.Add(role.Name!);

            return result;
        }

        public async Task<List<string>> GetUserRolesAsync(AppUserDto user)
        {
            var roles = (await _userManager.GetRolesAsync(FromDto(user))).ToList();
            return roles;
        }

        public async Task<bool> IsInRoleAsync(AppUserDto user, string role)
        {
            return await _userManager.IsInRoleAsync(FromDto(user), role);
        }

        public async Task<bool> AddToRoleAsync(Guid userId, string role)
        {
            // get user from id
            var user = await _userManager.Users.Where(u => u.Id == userId.ToString()).FirstOrDefaultAsync();

            if (user is null)
                return false;

            // check is user is already in role
            bool check = await _userManager.IsInRoleAsync(user, role);

            if (check == false)
            {
                // add to role
                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }
            else
                return false;  // already in role
        }

        public async Task<bool> RemoveFromRoleAsync(Guid userId, string role)
        {
            // get user from id
            var user = await _userManager.Users.Where(u => u.Id == userId.ToString()).FirstOrDefaultAsync();

            if (user is null)
                return false;

            // check if user is in role
            bool check = await _userManager.IsInRoleAsync(user, role);

            if (check == true)
            {
                // remove from role
                var result = await _userManager.RemoveFromRoleAsync(user, role);
                return result.Succeeded;
            }
            else
                return false;
        }

        public async Task<bool> DeleteUserAsync(AppUserDto user)
        {
            if (user is null)
                return false;

            // delete user
            // var result = await _userManager.DeleteAsync(FromDto(user));
            var existing = await _userManager.FindByIdAsync(user.Id);

            if (existing is null)
                return false;
            else
            {
                var result = await _userManager.DeleteAsync(existing);
                await _context.SaveChangesAsync();

                return result.Succeeded;
            }
        }

        private static AppUserDto ToDto(AppUser user)
        {
            // map AppUser to AppUserDto
            if (user is null)
                return null!;

            return new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                EmailConfirmed = user.EmailConfirmed
            };
        }
        private static AppUser FromDto(AppUserDto dto)
        {
            // map AppUserDto to AppUser
            return new AppUser
            {
                Id = dto.Id,
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed
            };
        }
    }
}
