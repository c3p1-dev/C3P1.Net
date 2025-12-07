using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface IBankAccountService
    {
        public Task<List<BankAccount>> GetBankAccountsAsync(Guid userId);
        public Task<bool> AddBankAccountAsync(Guid userId, BankAccount bankAccount);
        public Task<bool> DeleteBankAccountAsync(Guid userId, Guid bankAccountId);
    }
}
