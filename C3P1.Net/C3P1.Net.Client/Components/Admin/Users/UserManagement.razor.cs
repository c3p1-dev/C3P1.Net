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
                if (users is not null)
                {
                    var query = from u in users select u;
                    return query;
                }
                else
                    return [];
            }
        }

        [Inject]
        IUserService? UserService { get; set; }
        [Inject]
        IMessageService? MessageService { get; set; }

        protected async Task LoadData()
        {
            // create blank user list first time
            if (users is null)
                users = [];
            else // then clear the list next times
                users.Clear();

            // get all users
            var regularUsers = await UserService!.GetUsersAsync();
            var adminUsers = await UserService!.GetUsersInRoleAsync("Admin");

            foreach (var user in adminUsers)
            {
                // remove admin users from regular users list
                var u = regularUsers.Where(x => x.UserName == user.UserName).FirstOrDefault();

                if (u is not null)
                    regularUsers.Remove(u);
            }

            // combine both lists
            var result = adminUsers.Concat(regularUsers).ToList();

            foreach (var user in result)
            {
                AppUserWithRoles a = new()
                {
                    User = user,
                    Roles = await UserService!.GetUserRolesAsync(user)
                };

                // add to users list
                users.Add(a);
            }
        }

        protected async Task MakeAdmin(Guid userId)
        {
            // add to admin role
            await UserService!.AddToRoleAsync(userId, "Admin");
            await LoadData();

            // refresh UI
            await InvokeAsync(StateHasChanged);
        }

        protected async Task MakeRegular(Guid userId)
        {
            // remove from admin role
            await UserService!.RemoveFromRoleAsync(userId, "Admin");
            await LoadData();

            // refresh UI
            await InvokeAsync(StateHasChanged);
        }
        protected async Task DeleteUser(AppUserDto user)
        {
            // confirm delete
            if (await MessageService!.Confirm($"Are you sure you want to delete {user.Email} ?", "Confirmation"))
            {
                // delete user
                await UserService!.DeleteUserAsync(user);
                await LoadData();

                // refresh UI
                await InvokeAsync(StateHasChanged);
            }
        }

    }
}
