using C3P1.Net.Data;

namespace C3P1.Net.Services.Admin
{
    public interface IUsersAdminService
    {
        public Task<List<ApplicationUser>> GetAdminUsersAsync();
        public Task<List<ApplicationUser>> GetRegularUsersAsync();
        public Task<List<ApplicationUser>> GetUsersAsync();
        public Task<bool> DeleteUserAsync(ApplicationUser user);
        public Task<bool> MakeAdminAsync(ApplicationUser user);
        public Task<bool> MakeRegularAsync(ApplicationUser user);
        public Task<bool> IsAdminAsync(ApplicationUser user);
    }
}
