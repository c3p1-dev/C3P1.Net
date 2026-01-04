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
    public class SubCategoryController(ISubCategoryService subcategoryService, UserManager<AppUser> userManager) : ControllerBase
    {
        // GET api/apps/bankbook/[controller]/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get all subcategories for the current user
            var result = await subcategoryService.GetSubCategoriesAsync(currentUserId);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // POST api/apps/bankbook/[controller]/add
        // data [FromBody]
        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddSubCategoryAsync([FromBody] SubCategory subCategory)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // duplicate check
            var existingSubCategory = await subcategoryService.GetSubCategoryByCodeAsync(currentUserId, subCategory.Code);
            if (existingSubCategory is not null)
                return Conflict("A subcategory with the same code already exists.");  // subcategory with the same code already exists

            // add subcategory for the current user
            bool result = await subcategoryService.AddSubCategoryAsync(currentUserId, subCategory);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // DELETE api/apps/bankbook/[controller]/delete/{id}
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteSubCategoryAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // delete subcategory from id
            bool result = await subcategoryService.DeleteSubCategoryAsync(currentUserId, id);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // UPDATE api/apps/bankbook/[controller]/update
        [HttpPut("update")]
        public async Task<ActionResult<bool>> UpdateSubCategoryAsync([FromBody] SubCategory subCategory)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);
            bool result = await subcategoryService.UpdateSubCategoryAsync(currentUserId, subCategory);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // GET api/apps/bankbook/[controller]/get/{id}
        [HttpGet("get/{id:Guid}")]
        public async Task<ActionResult<SubCategory>> GetSubCategoryByIdAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get subcategory by id for the current user
            var result = await subcategoryService.GetSubCategoryByIdAsync(currentUserId, id);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/get/code/{code}
        [HttpGet("get/code/{code}")]
        public async Task<ActionResult<SubCategory>> GetSubCategoryByCodeAsync(string code)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get subcategory by code for the current user
            var result = await subcategoryService.GetSubCategoryByCodeAsync(currentUserId, code);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/list/category/{categoryId}
        [HttpGet("list/category/{categoryId:Guid}")]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesByCategoryIdAsync(Guid categoryId)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get subcategories by category id for the current user
            var result = await subcategoryService.GetSubCategoriesByCategoryIdAsync(currentUserId, categoryId);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/list/category/code/{categoryCode}
        [HttpGet("list/category/code/{categoryCode}")]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesByCategoryCodeAsync(string categoryCode)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get subcategories by category code for the current user
            var result = await subcategoryService.GetSubCategoriesByCategoryCodeAsync(currentUserId, categoryCode);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}