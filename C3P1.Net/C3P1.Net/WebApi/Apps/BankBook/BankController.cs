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
    public class BankController(IBankAccountService bankAccountService, UserManager<AppUser> userManager) : ControllerBase
    {


        // GET : api/apps/bankbook/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccountsAsync()
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

            // get all bank accounts for the current user
            var result = await bankAccountService.GetBankAccountsAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // POST : api/apps/bankbook/[controller]
        // data [FromBody]
        [HttpPost]
        public async Task<ActionResult<bool>> AddBankAccountAsync([FromBody] BankAccount bankAccount)
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

            bool result = await bankAccountService.AddBankAccountAsync(currentUserId, bankAccount);

            if (result)
                return Ok(result);
            else
                return BadRequest();
        }

        // DELETE : api/apps/[controller]/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteBankAccountAsync(Guid id)
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

            // delete task from id
            var result = await bankAccountService.DeleteBankAccountAsync(currentUserId, id);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
