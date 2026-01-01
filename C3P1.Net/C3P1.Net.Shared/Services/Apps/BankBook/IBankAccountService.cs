using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface IBankAccountService
    {
        public Task<List<BankAccount>> GetBankAccountsAsync(Guid userId);
        public Task<bool> AddBankAccountAsync(Guid userId, BankAccount bankAccount);
        public Task<bool> DeleteBankAccountAsync(Guid userId, Guid bankAccountId);
        public Task<bool> UpdateBankAccountAsync(Guid userId, BankAccount bankAccount);
        public Task<BankAccount?> GetBankAccountByIdAsync(Guid userId, Guid bankAccountId);
    }
}
