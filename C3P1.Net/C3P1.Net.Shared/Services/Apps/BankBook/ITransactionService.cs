using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface ITransactionService
    {
        public Task<List<Transaction>> GetTransactionsAsync(Guid userId);
        public Task<bool> AddTransactionAsync(Guid userId, Transaction transaction);
        public Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId);
        public Task<bool> UpdateTransactionAsync(Guid userId, Transaction transaction);
        public Task<Transaction?> GetTransactionByIdAsync(Guid userId, Guid transactionId);
        public Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid userId, Guid accountId);
        public Task<List<Transaction>> GetTransactionsBySubCategoryIdAsync(Guid userId, Guid subCategoryId);
    }
}
