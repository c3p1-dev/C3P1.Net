using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps.BankBook
{
    public class AccountServerService(BankBookDbContext context) : IAccountService
    {
        private readonly BankBookDbContext _context = context;
        public async Task<List<Account>> GetAccountsAsync(Guid userId)
        {
            var result = await _context.Accounts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<bool> AddAccountAsync(Guid userId, Account bankAccount)
        {
            // fill data
            bankAccount.Id = Guid.NewGuid();
            bankAccount.UserId = userId;
            // add bank account
            _context.Add(bankAccount);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> DeleteAccountAsync(Guid userId, Guid bankAccountId)
        {
            var bankAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);
            if (bankAccount != null)
            {
                _context.Accounts.Remove(bankAccount);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }

        public async Task<bool> UpdateAccountAsync(Guid userId, Account bankAccount)
        {
            var existingBankAccount = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Id && x.UserId == userId);
            if (existingBankAccount != null)
            {
                bankAccount.UserId = userId; // ensure the userId is not changed
                _context.Accounts.Update(bankAccount);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }

        public async Task<Account?> GetAccountByIdAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);
            return result;
        }
    }
}
