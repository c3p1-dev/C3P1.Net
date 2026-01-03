using Blazorise;
using C3P1.Net.Shared.Data;
using C3P1.Net.Shared.Data.Admin.UserManagement;
using C3P1.Net.Shared.Services.Admin;
using Microsoft.AspNetCore.Components;

namespace C3P1.Net.Client.Components.Admin.UserManagement
{
    public abstract class UserManagementBase : ComponentBase
    {
        protected List<AppUserWithRoles>? users;
        protected IEnumerable<AppUserWithRoles> Users
        {
            get
            {
                if (users != null)
                {
                    var query = from u in users select u;
                    return query;
                }
                else
                {
                    return [];
                }
            }
        }

        [Inject]
        IUserService? UserManagementService { get; set; }
        [Inject]
        IMessageService? MessageService { get; set; }

        protected async Task LoadData()
        {
            // create blank user list first time
            if (users == null)
            {
                users = [];
            }
            else // then clear the list next times
            {
                users.Clear();
            }

            var regularUsers = await UserManagementService!.GetUsersAsync();
            var adminUsers = await UserManagementService!.GetUsersInRoleAsync("Admin");

            foreach (var user in adminUsers)
            {
                var u = regularUsers.Where(x => x.UserName == user.UserName).FirstOrDefault();
                if (u != null)
                {
                    regularUsers.Remove(u);
                }
            }

            var result = adminUsers.Concat(regularUsers).ToList();

            foreach (var user in result)
            {
                AppUserWithRoles a = new()
                {
                    User = user,
                    Roles = await UserManagementService!.GetUserRolesAsync(user)
                };

                users.Add(a);
            }
        }

        protected async Task MakeAdmin(Guid userId)
        {
            await UserManagementService!.AddToRoleAsync(userId, "Admin");
            await LoadData();

            await InvokeAsync(StateHasChanged);
        }

        protected async Task MakeRegular(Guid userId)
        {
            await UserManagementService!.RemoveFromRoleAsync(userId, "Admin");
            await LoadData();

            await InvokeAsync(StateHasChanged);
        }
        protected async Task DeleteUser(AppUserDto user)
        {
            if (await MessageService!.Confirm($"Are you sure you want to delete {user.Email} ?", "Confirmation"))
            {
                await UserManagementService!.DeleteUserAsync(user);
                await LoadData();

                await InvokeAsync(StateHasChanged);
            }
        }

    }
}
