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
    public class AccountController(IAccountService bankAccountService, UserManager<AppUser> userManager) : ControllerBase
    {


        // GET : api/apps/bankbook/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetBankAccountsAsync()
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
            var result = await bankAccountService.GetAccountsAsync(currentUserId);

            if (result == null)
                return BadRequest();
            else
                return Ok(result);
        }

        // POST : api/apps/bankbook/[controller]
        // data [FromBody]
        [HttpPost]
        public async Task<ActionResult<bool>> AddBankAccountAsync([FromBody] Account bankAccount)
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

            bool result = await bankAccountService.AddAccountAsync(currentUserId, bankAccount);

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
            var result = await bankAccountService.DeleteAccountAsync(currentUserId, id);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET : api/apps/bankbook/[controller]/{id}
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Account>> GetBankAccountByIdAsync(Guid id)
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
            var result = await bankAccountService.GetAccountByIdAsync(currentUserId, id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        // PUT : api/apps/bankbook/[controller]
        [HttpPut]
        public async Task<ActionResult<bool>> UpdateBankAccountAsync([FromBody] Account bankAccount)
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
            bool result = await bankAccountService.UpdateAccountAsync(currentUserId, bankAccount);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }
    }
}
