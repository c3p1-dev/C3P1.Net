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
        // GET api/apps/bankbook/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetSubCategoriesAsync()
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

            // get all subcategories for the current user
            var result = await subcategoryService.GetSubCategoriesAsync(currentUserId);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }

        // POST api/apps/bankbook/[controller]
        // data [FromBody]
        [HttpPost]
        public async Task<ActionResult<bool>> AddSubCategoryAsync([FromBody] SubCategory subCategory)
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

            bool result = await subcategoryService.AddSubCategoryAsync(currentUserId, subCategory);
            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // DELETE api/apps/bankbook/[controller]/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteSubCategoryAsync(Guid id)
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

            // delete subcategory from id
            bool result = await subcategoryService.DeleteSubCategoryAsync(currentUserId, id);
            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // UPDATE api/apps/bankbook/[controller]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateSubCategoryAsync([FromBody] SubCategory subCategory)
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
            bool result = await subcategoryService.UpdateSubCategoryAsync(currentUserId, subCategory);
            if (result)
                return Ok(true);
            else
                return BadRequest();
        }

        // GET api/apps/bankbook/[controller]/{id}
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<SubCategory>> GetSubCategoryByIdAsync(Guid id)
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

            // get subcategory by id for the current user
            var result = await subcategoryService.GetSubCategoryByIdAsync(currentUserId, id);
            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}