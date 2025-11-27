using C3P1.Net.Client.Data;
using C3P1.Net.Client.Data.Admin.UserManagement;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Admin
{
    public class UserManagementClientService(HttpClient httpClient) : IUserManagementService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<AppUser>> GetUsersAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<AppUser>>("api/admin/manageuser/list/users");

            if (result != null)
            {
                return result;
            }
            else
            {
                return [];
            }
        }
        public async Task<List<AppUser>> GetUsersInRoleAsync(string role)
        {
            var result = await _httpClient.GetFromJsonAsync<List<AppUser>>("api/admin/manageuser/list/inrole/" + role);

            if (result != null)
            {
                return result;
            }
            else
            {
                return [];
            }
        }
        public async Task<List<string>> GetRolesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<string>>("api/admin/manageuser/list/roles");

            if (result != null)
            {
                return result;
            }
            else
            {
                return [];
            }
        }

        public async Task<List<string>> GetUserRolesAsync(AppUser user)
        {
            var result = await _httpClient.PostAsJsonAsync<AppUser>("api/admin/manageuser/user/roles", user);
            var roles = await result.Content.ReadFromJsonAsync<List<string>>();

            if (roles != null)
            {
                return roles;
            }
            else
            {
                return [];
            }
        }
        public async Task<bool> IsInRoleAsync(AppUser user, string role)
        {
            RoleEditModel data = new() { UserId = Guid.Parse(user.Id), Role = role };
            var result = await _httpClient.PostAsJsonAsync("api/admin/manageuser/user/inrole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> AddToRoleAsync(Guid userId, string role)
        {
            RoleEditModel data = new() { Role = role, UserId = userId };
            var result = await _httpClient.PostAsJsonAsync<RoleEditModel>("api/admin/manageuser/user/addrole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> RemoveFromRoleAsync(Guid userId, string role)
        {
            RoleEditModel data = new() { Role = role, UserId = userId };
            var result = await _httpClient.PostAsJsonAsync<RoleEditModel>("api/admin/manageuser/user/removerole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(AppUser user)
        {
            var result = await _httpClient.PostAsJsonAsync<AppUser>("api/admin/manageuser/user/delete", user);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
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
