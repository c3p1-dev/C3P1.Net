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
    public class TransactionController(ITransactionService transactionService, UserManager<AppUser> userManager) : ControllerBase
    {
        // GET api/apps/bankbook/[controller]/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsAsync()
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            // get all transactions for the current user
            var result = await transactionService.GetTransactionsAsync(currentUserId);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/list/account/{accountId}
        [HttpGet("list/account/{accountId:Guid}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            // get all transactions for the current user and the count id accountId
            var result = await transactionService.GetTransactionsByAccountIdAsync(currentUserId, accountId);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // GET api/apps/bankbook/[controller]/list/subcategory/{subcategoryId}
        [HttpGet("list/subcategory/{subcategoryId:Guid}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsBySubCategoryIdAsync(Guid subcategoryId)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            // get all transactions for the current user and the subcategory id
            var result = await transactionService.GetTransactionsBySubCategoryIdAsync(currentUserId, subcategoryId);

            if (result is not null)
                return Ok(result);
            else
                return NotFound();
        }

        // POST api/apps/bankbook/[controller]/add
        // data [FromBody]
        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddTransaction([FromBody] Transaction transaction)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            // add transaction for the current user
            bool result = await transactionService.AddTransactionAsync(currentUserId, transaction);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // DELETE api/apps/bankbook/[controller]/delete/{id}
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult<bool>> DeleteTransaction(Guid id)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            // delete transaction from id
            bool result = await transactionService.DeleteTransactionAsync(currentUserId, id);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }

        // UPDATE api/apps/bankbook/[controller]/update
        // data [FromBody]
        [HttpPut("update")]
        public async Task<ActionResult<bool>> UpdateTransaction([FromBody] Transaction transaction)
        {
            // get user id
            var name = User.Identity?.Name;
            var user = await userManager.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized();

            var currentUserId = Guid.Parse(user.Id);

            bool result = await transactionService.UpdateTransactionAsync(currentUserId, transaction);

            if (result == true)
                return Ok(result);
            else
                return BadRequest();
        }
    }
}
