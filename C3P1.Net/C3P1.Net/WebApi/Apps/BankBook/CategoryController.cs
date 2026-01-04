using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.WebApi.Apps.BankBook
{
    [Authorize]
    [Route("api/apps/bankbook/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService, UserManager<AppUser> userManager) : ControllerBase
    {
        // GET : api/apps/bankbook/[controller]/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Account>>> GetCategoriesAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen due to [Authorize] attribute
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // get all categories for the current user
            var result = await categoryService.GetCategoriesAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // POST : api/apps/bankbook/[controller]/add
        // data [FromBody]
        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddCategoryAsync([FromBody] Category category)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // add category
            bool result = await categoryService.AddCategoryAsync(currentUserId, category);

            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // DELETE : api/apps/bankbook/[controller]/delete/{id}
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // delete category from id
            bool result = await categoryService.DeleteCategoryAsync(currentUserId, id);

            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // UPDATE : api/apps/bankbook/[controller]/update
        [HttpPut("update")]
        public async Task<ActionResult<bool>> UpdateCategoryAsync(Guid id, [FromBody] Category category)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // update category
            bool result = await categoryService.UpdateCategoryAsync(currentUserId, category);

            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // GET api/apps/bankbook/[controller]/get/{id}
        [HttpGet("get/{id:Guid}")]
        public async Task<ActionResult<Category>> GetCategoryByIdAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            if (user == null)
            {
                // should not happen due to [Authorize] attribute
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(user.Id);

            // get category by id for the current user
            var result = await categoryService.GetCategoryByIdAsync(currentUserId, id);

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

    }
}