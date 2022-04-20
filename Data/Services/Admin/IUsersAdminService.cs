using C3P1.Net.Identity.Data;

namespace C3P1.Net.Data.Services.Admin
{
    public interface IUsersAdminService
    {
        public Task<List<C3P1User>> GetAdminUsersAsync();
        public Task<List<C3P1User>> GetRegularUsersAsync();
        public Task<List<C3P1User>> GetUsersAsync();
        public Task<bool> DeleteUserAsync(C3P1User user);
        public Task<bool> MakeAdminAsync(C3P1User user);
        public Task<bool> MakeRegularAsync(C3P1User user);
    }
}
