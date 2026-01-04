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
            // get all bank accounts for the user
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

            // duplicate check
            var duplicateAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == bankAccount.Code);

            if (duplicateAccount is null)
            {

                // add bank account
                _context.Add(bankAccount);
                int recorded = await _context.SaveChangesAsync();

                return (recorded == 1);
            }
            else
                return false; // duplicate found
        }

        public async Task<bool> DeleteAccountAsync(Guid userId, Guid bankAccountId)
        {
            // find bank account
            var bankAccount = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);

            // delete if found
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
            // find existing bank account
            var existingBankAccount = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Id && x.UserId == userId);

            // update if found
            if (existingBankAccount != null)
            {
                // duplicate check
                var duplicateAccount = await _context.Accounts
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == bankAccount.Code && x.Id != bankAccount.Id);
                if (duplicateAccount is null)
                {
                    // ensure the userId is not changed
                    bankAccount.UserId = userId;
                    _context.Accounts.Update(bankAccount);
                    int recorded = await _context.SaveChangesAsync();
                    return (recorded == 1);
                }
                else
                    return false; // duplicate found
            }

            return false; // not found
        }

        public async Task<Account?> GetAccountByIdAsync(Guid userId, Guid bankAccountId)
        {
            // get bank account by id
            var result = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == bankAccountId && x.UserId == userId);

            return result;
        }

        public async Task<Account?> GetAccountByCodeAsync(Guid userId, string code)
        {
            // get bank account by code
            var result = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Code == code && x.UserId == userId);

            return result;
        }
    }
}
