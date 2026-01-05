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
            if (bankAccount is not null)
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
                .FirstOrDefaultAsync(x => x.Id == bankAccount.Id && x.UserId == userId);

            // if not found, return false
            if (existingBankAccount is null)
                return false;

            // check for duplicate code
            var duplicateAccount = await _context.Accounts
                .AnyAsync(x => x.UserId == userId
                            && x.Code == bankAccount.Code
                            && x.Id != bankAccount.Id);

            // if duplicate found, return false
            if (duplicateAccount == true)
                return false;

            // update fields
            existingBankAccount.Code = bankAccount.Code;
            existingBankAccount.Name = bankAccount.Name;
            existingBankAccount.IBAN = bankAccount.IBAN;
            existingBankAccount.Swift = bankAccount.Swift;
            existingBankAccount.Bank = bankAccount.Bank;
            existingBankAccount.Description = bankAccount.Description;
            existingBankAccount.Url = bankAccount.Url;
            existingBankAccount.Balance = bankAccount.Balance;
            existingBankAccount.LockedAt = bankAccount.LockedAt;

            // save changes
            await _context.SaveChangesAsync();
            return true;
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
