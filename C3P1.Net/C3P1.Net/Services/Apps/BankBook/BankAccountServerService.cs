using C3P1.Net.Client.Data.Apps.BankBook;
using C3P1.Net.Client.Services.Apps.BankBook;
using C3P1.Net.Data;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps.BankBook
{
    public class BankAccountServerService(BankBookDbContext context) : IBankAccountService
    {
        private readonly BankBookDbContext _context = context;
        public async Task<List<BankAccount>> GetBankAccountsAsync(Guid userId)
        {
            var result = await _context.BankAccounts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<bool> AddBankAccountAsync(Guid userId, BankAccount bankAccount)
        {
            // fill data
            bankAccount.Id = Guid.NewGuid();
            bankAccount.UserId = userId;
            // add bank account
            _context.Add(bankAccount);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }
    }
}
