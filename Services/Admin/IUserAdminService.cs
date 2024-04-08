using C3P1.Net.Data;

namespace C3P1.Net.Services.Admin
{
    public interface IUserAdminService
    {
        public Task<List<AppUser>> GetAdminUsersAsync();
        public Task<List<AppUser>> GetRegularUsersAsync();
        public Task<List<AppUser>> GetUsersAsync();
        public Task<bool> DeleteUserAsync(AppUser user);
        public Task<bool> MakeAdminAsync(AppUser user);
        public Task<bool> MakeRegularAsync(AppUser user);
        public Task<bool> IsAdminAsync(AppUser user);
    }
}
