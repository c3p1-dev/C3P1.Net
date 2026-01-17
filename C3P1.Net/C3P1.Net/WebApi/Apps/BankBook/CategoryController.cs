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

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get all categories for the current user
            var result = await categoryService.GetCategoriesAsync(currentUserId);

            if (result is null)
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

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // duplicate check
            var existingCategory = await categoryService.GetCategoryByCodeAsync(currentUserId, category.Code);

            if (existingCategory is not null)
                return Conflict("A category with the same code already exists");  // category with the same code already exists

            // add category
            bool result = await categoryService.AddCategoryAsync(currentUserId, category);

            if (result == true)
                return Ok(result);
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

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // delete category from id
            bool result = await categoryService.DeleteCategoryAsync(currentUserId, id);

            if (result == true)
                return Ok(result);
            else
                return BadRequest("Category does not exist or has SubCategories bound to it");
        }

        // UPDATE : api/apps/bankbook/[controller]/update
        [HttpPut("update")]
        public async Task<ActionResult<bool>> UpdateCategoryAsync(Guid id, [FromBody] Category category)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // duplicate check
            var existingCategory = await categoryService.GetCategoryByCodeAsync(currentUserId, category.Code);

            if (existingCategory != null && existingCategory.Id != category.Id)
                return Conflict("A category with the same code already exists");  // category with the same code already exists

            // update category
            bool result = await categoryService.UpdateCategoryAsync(currentUserId, category);

            if (result == true)
                return Ok(result);
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

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get category by id for the current user
            var result = await categoryService.GetCategoryByIdAsync(currentUserId, id);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/get/code/{code}
        [HttpGet("get/code/{code}")]
        public async Task<ActionResult<Category>> GetCategoryByCodeAsync(string code)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get category by code for the current user
            var result = await categoryService.GetCategoryByCodeAsync(currentUserId, code);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

    }
}