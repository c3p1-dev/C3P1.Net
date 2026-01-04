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


        // GET : api/apps/bankbook/[controller]/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountsAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen due to [Authorize] attribute

            var currentUserId = Guid.Parse(user.Id);

            // get all bank accounts for the current user
            var result = await bankAccountService.GetAccountsAsync(currentUserId);

            if (result is not null)
                return Ok(result);
            else
                return BadRequest();
        }

        // POST : api/apps/bankbook/[controller]/add
        // data [FromBody]
        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddAccountAsync([FromBody] Account bankAccount)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // duplicate check
            var existingAccount = await bankAccountService.GetAccountByCodeAsync(currentUserId, bankAccount.Code);

            if (existingAccount is not null)
                return Conflict("An account with the same code already exists.");

            // add bank account
            bool result = await bankAccountService.AddAccountAsync(currentUserId, bankAccount);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // DELETE : api/apps/[controller]/delete/{id}
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteAccountAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // delete task from id
            var result = await bankAccountService.DeleteAccountAsync(currentUserId, id);

            if (result == true)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // GET : api/apps/bankbook/[controller]/get/{id}
        [HttpGet("get/{id:Guid}")]
        public async Task<ActionResult<Account>> GetAccountByIdAsync(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // get bank account from id
            var result = await bankAccountService.GetAccountByIdAsync(currentUserId, id);

            if (result is null)
                return NotFound();
            else
                return Ok(result);
        }

        // GET : api/apps/bankbook/[controller]/code/{code}
        [HttpGet("get/code/{code}")]
        public async Task<ActionResult<Account>> GetAccountByCodeAsync(string code)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // get bank account from code
            var result = await bankAccountService.GetAccountByCodeAsync(currentUserId, code);

            if (result is null)
                return NotFound();
            else
                return Ok(result);
        }

        // PUT : api/apps/bankbook/[controller]/update
        [HttpPut("update")]
        public async Task<ActionResult<bool>> UpdateAccountAsync([FromBody] Account bankAccount)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();  // should not happen

            var currentUserId = Guid.Parse(user.Id);

            // duplicate check
            var existingAccount = await bankAccountService.GetAccountByCodeAsync(currentUserId, bankAccount.Code);

            if (existingAccount is not null && existingAccount.Id != bankAccount.Id)
                return Conflict("An account with the same code already exists.");

            // update bank account
            bool result = await bankAccountService.UpdateAccountAsync(currentUserId, bankAccount);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }
    }
}
