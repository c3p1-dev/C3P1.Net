using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class TransactionClientService(HttpClient httpClient) : ITransactionService
    {
        public async Task<bool> AddTransactionAsync(Guid userId, Transaction transaction)
        {
            var result = await httpClient.PostAsJsonAsync($"/api/apps/bankbook/transaction/add", transaction);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId)
        {
            var result = await httpClient.DeleteAsync($"/api/apps/bankbook/transaction/delete/{transactionId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid userId, Guid transactionId)
        {
            var result = await httpClient.GetFromJsonAsync<Transaction?>($"/api/apps/bankbook/transaction/get/{transactionId}");

            return result;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(Guid userId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/apps/bankbook/transaction/list");

            return result!;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid userId, Guid accountId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/apps/bankbook/transaction/list/account/{accountId}");

            return result!;
        }

        public async Task<List<Transaction>> GetTransactionsBySubCategoryIdAsync(Guid userId, Guid subCategoryId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/apps/bankbook/transaction/list/subcategory/{subCategoryId}");

            return result!;
        }

        public async Task<bool> UpdateTransactionAsync(Guid userId, Transaction transaction)
        {
            var result = await httpClient.PutAsJsonAsync($"/api/apps/bankbook/transaction/update", transaction);

            return result.IsSuccessStatusCode;
        }
    }
}
