using C3P1.Net.Shared.Data;
using C3P1.Net.Shared.Data.Admin.UserManagement;
using C3P1.Net.Shared.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace C3P1.Net.WebApi.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UserController(IUserService userManagementService) : ControllerBase
    {

        // GET : api/admin/[controller]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsersAsync()
        {
            var result = await userManagementService.GetUsersAsync();

            if (result is not null)
                return Ok(result);
            else
                return BadRequest();
        }

        // GET : api/admin/[controller]/list/inrole/{role}
        [HttpGet("list/inrole/{role}")]
        //public async Task<ActionResult<IEnumerable<TodoItem>>> GetUsersInRoleAsync(string role)
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsersInRoleAsync(string role)
        {
            var result = await userManagementService.GetUsersInRoleAsync(role);

            if (result is not null)
                return Ok(result);
            else
                return BadRequest();
        }

        // GET : api/admin/[controller]/list/roles
        [HttpGet("list/roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRolesAsync()
        {
            var result = await userManagementService.GetRolesAsync();

            if (result is not null)
                return Ok(result);
            else
                return BadRequest();
        }

        // POST : api/admin/[controller]/user/roles
        [HttpPost("roles")]
        public async Task<ActionResult<List<string>>> GetUserRolesAsync([FromBody] AppUserDto user)
        {
            var result = await userManagementService.GetUserRolesAsync(user);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/inrole/{role}
        [HttpPost("inrole")]
        public async Task<ActionResult<bool>> IsInRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            var user = (await userManagementService.GetUsersAsync())
                .Where(u => u.Id == roleEditModel.UserId.ToString())
                .FirstOrDefault();

            if (user is not null)
            {
                bool result = await userManagementService.IsInRoleAsync(user, roleEditModel.Role!);
                return Ok(result);
            }
            else
                return BadRequest();

        }

        // POST : api/admin/[controller]/user/addrole/{role}
        [HttpPost("addrole")]
        public async Task<ActionResult<bool>> AddToRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            bool result = await userManagementService.AddToRoleAsync(roleEditModel.UserId, roleEditModel.Role!);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/removerole/{role}
        [HttpPost("removerole")]
        public async Task<ActionResult<bool>> RemoveFromRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            bool result = await userManagementService.RemoveFromRoleAsync(roleEditModel.UserId, roleEditModel.Role!);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/delete
        [HttpPost("delete")]
        public async Task<ActionResult<bool>> DeleteUserAsync([FromBody] AppUserDto user)
        {
            bool result = await userManagementService.DeleteUserAsync(user);

            return result;
        }
    }
}
