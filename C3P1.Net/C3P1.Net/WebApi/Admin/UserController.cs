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
        private readonly IUserService _manageUserService = userManagementService;

        // GET : api/admin/[controller]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsersAsync()
        {
            var result = await _manageUserService.GetUsersAsync();

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        // GET : api/admin/[controller]/list/inrole/{role}
        [HttpGet("list/inrole/{role}")]
        //public async Task<ActionResult<IEnumerable<TodoItem>>> GetUsersInRoleAsync(string role)
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsersInRoleAsync(string role)
        {
            var result = await _manageUserService.GetUsersInRoleAsync(role);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        // GET : api/admin/[controller]/list/roles
        [HttpGet("list/roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRolesAsync()
        {
            var result = await _manageUserService.GetRolesAsync();

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        // POST : api/admin/[controller]/user/roles
        [HttpPost("roles")]
        public async Task<ActionResult<List<string>>> GetUserRolesAsync([FromBody] AppUserDto user)
        {
            var result = await _manageUserService.GetUserRolesAsync(user);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/inrole/{role}
        [HttpPost("inrole")]
        public async Task<ActionResult<bool>> IsInRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            var user = (await _manageUserService.GetUsersAsync())
                .Where(u => u.Id == roleEditModel.UserId.ToString())
                .FirstOrDefault();

            if (user != null)
            {
                bool result = await _manageUserService.IsInRoleAsync(user, roleEditModel.Role!);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        // POST : api/admin/[controller]/user/addrole/{role}
        [HttpPost("addrole")]
        public async Task<ActionResult<bool>> AddToRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            bool result = await _manageUserService.AddToRoleAsync(roleEditModel.UserId, roleEditModel.Role!);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/removerole/{role}
        [HttpPost("removerole")]
        public async Task<ActionResult<bool>> RemoveFromRoleAsync([FromBody] RoleEditModel roleEditModel)
        {
            bool result = await _manageUserService.RemoveFromRoleAsync(roleEditModel.UserId, roleEditModel.Role!);

            return Ok(result);
        }

        // POST : api/admin/[controller]/user/delete
        [HttpPost("delete")]
        public async Task<ActionResult<bool>> DeleteUserAsync([FromBody] AppUserDto user)
        {
            bool result = await _manageUserService.DeleteUserAsync(user);

            return result;
        }
    }
}
