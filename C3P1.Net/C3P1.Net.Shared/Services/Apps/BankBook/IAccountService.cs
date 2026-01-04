using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface IAccountService
    {
        public Task<List<Account>> GetAccountsAsync(Guid userId);
        public Task<bool> AddAccountAsync(Guid userId, Account bankAccount);
        public Task<bool> DeleteAccountAsync(Guid userId, Guid bankAccountId);
        public Task<bool> UpdateAccountAsync(Guid userId, Account bankAccount);
        public Task<Account?> GetAccountByIdAsync(Guid userId, Guid bankAccountId);
        public Task<Account?> GetAccountByCodeAsync(Guid userId, string code);
    }
}
