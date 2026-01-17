using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps.BankBook
{
    public class TransactionServerService(BankBookDbContext context) : ITransactionService
    {
        private readonly BankBookDbContext _context = context;
        public async Task<bool> AddTransactionAsync(Guid userId, Transaction transaction)
        {
            // fill data
            transaction.Id = Guid.NewGuid();
            transaction.UserId = userId;
            transaction.CreatedAt = DateTime.UtcNow;

            // add to db
            _context.Add(transaction);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1) ;
        }

        public async Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId)
        {
            // find transaction
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.UserId == userId);

            // delete if found
            if (transaction is not null)
            {
                _context.Remove(transaction);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid userId, Guid transactionId)
        {
            // find transaction
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.UserId == userId);

            return transaction;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(Guid userId)
        {
            // get transactions
            var result = await _context.Transactions
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid userId, Guid accountId)
        {
            // get transactions by account id
            var result = await _context.Transactions
                .Where(x => x.UserId == userId && x.AccountId == accountId)
                .ToListAsync();

            return result;
        }

        public async Task<List<Transaction>> GetTransactionsBySubCategoryIdAsync(Guid userId, Guid subCategoryId)
        {
            // get transactions by subcategory id
            var result = await _context.Transactions
                .Where(x => x.UserId == userId && x.SubCategoryId == subCategoryId)
                .ToListAsync();
            return result;
        }

        public async Task<bool> UpdateTransactionAsync(Guid userId, Transaction transaction)
        {
            // find existing transaction
            var existingTransaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == transaction.Id && x.UserId == userId);
            if (existingTransaction is not null)
            {
                // update fields
                existingTransaction.AccountingDate = transaction.AccountingDate;
                existingTransaction.ValueDate = transaction.ValueDate;
                existingTransaction.Label = transaction.Label;
                existingTransaction.Note = transaction.Note;
                existingTransaction.PaymentMethod = transaction.PaymentMethod;
                existingTransaction.IsReconciled = transaction.IsReconciled;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.SubCategoryId = transaction.SubCategoryId;
                existingTransaction.CheckNumber = transaction.CheckNumber;

                // save changes
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }
    }
}
