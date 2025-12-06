using C3P1.Net.Shared.Data;

namespace C3P1.Net.Shared.Services.Admin
{
    public interface IUserManagementService
    {
        public Task<List<AppUserDto>> GetUsersAsync();
        public Task<List<AppUserDto>> GetUsersInRoleAsync(string role);
        public Task<List<string>> GetRolesAsync();
        public Task<List<string>> GetUserRolesAsync(AppUserDto user);
        public Task<bool> IsInRoleAsync(AppUserDto user, string role);
        public Task<bool> AddToRoleAsync(Guid userId, string role);
        public Task<bool> RemoveFromRoleAsync(Guid userId, string role);
        public Task<bool> DeleteUserAsync(AppUserDto user);
    }
}
