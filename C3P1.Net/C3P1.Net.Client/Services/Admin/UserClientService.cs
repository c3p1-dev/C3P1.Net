using C3P1.Net.Shared.Data;
using C3P1.Net.Shared.Data.Admin.UserManagement;
using C3P1.Net.Shared.Services.Admin;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Admin
{
    public class UserClientService(HttpClient httpClient) : IUserService
    {
        public async Task<List<AppUserDto>> GetUsersAsync()
        {
            var result = await httpClient.GetFromJsonAsync<List<AppUserDto>>("api/admin/user/list");

            if (result is not null)
                return result;
            else
                return [];
        }
        public async Task<List<AppUserDto>> GetUsersInRoleAsync(string role)
        {
            var result = await httpClient.GetFromJsonAsync<List<AppUserDto>>("api/admin/user/list/inrole/" + role);

            if (result is not null)
                return result;
            else
                return [];
        }
        public async Task<List<string>> GetRolesAsync()
        {
            var result = await httpClient.GetFromJsonAsync<List<string>>("api/admin/user/list/roles");

            if (result is not null)
                return result;
            else
                return [];
        }

        public async Task<List<string>> GetUserRolesAsync(AppUserDto user)
        {
            var result = await httpClient.PostAsJsonAsync<AppUserDto>("api/admin/user/roles", user);
            var roles = await result.Content.ReadFromJsonAsync<List<string>>();

            if (roles is not null)
                return roles;
            else
                return [];
        }
        public async Task<bool> IsInRoleAsync(AppUserDto user, string role)
        {
            RoleEditModel data = new() { UserId = Guid.Parse(user.Id), Role = role };
            var result = await httpClient.PostAsJsonAsync("api/admin/user/inrole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
                return true;
            else
                return false;
        }
        public async Task<bool> AddToRoleAsync(Guid userId, string role)
        {
            RoleEditModel data = new() { Role = role, UserId = userId };
            var result = await httpClient.PostAsJsonAsync<RoleEditModel>("api/admin/user/addrole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
                return true;
            else
                return false;
        }

        public async Task<bool> RemoveFromRoleAsync(Guid userId, string role)
        {
            RoleEditModel data = new() { Role = role, UserId = userId };
            var result = await httpClient.PostAsJsonAsync<RoleEditModel>("api/admin/user/removerole", data);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteUserAsync(AppUserDto user)
        {
            var result = await httpClient.PostAsJsonAsync<AppUserDto>("api/admin/user/delete", user);

            var success = await result.Content.ReadAsStringAsync();

            if (success == "true")
                return true;
            else
                return false;
        }
    }
}
