using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
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

        public async Task<bool> DeleteBankAccountAsync(Guid userId, Guid bankAccountId)
        {
            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);
            if (bankAccount != null)
            {
                _context.BankAccounts.Remove(bankAccount);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }

        public async Task<bool> UpdateBankAccountAsync(Guid userId, BankAccount bankAccount)
        {
            var existingBankAccount = await _context.BankAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Id && x.UserId == userId);
            if (existingBankAccount != null)
            {
                bankAccount.UserId = userId; // ensure the userId is not changed
                _context.BankAccounts.Update(bankAccount);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }

        public async Task<BankAccount?> GetBankAccountByIdAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _context.BankAccounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);
            return result;
        }
    }
}
