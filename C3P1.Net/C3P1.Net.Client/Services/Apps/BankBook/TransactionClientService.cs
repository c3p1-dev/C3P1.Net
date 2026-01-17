using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class TransactionClientService(HttpClient httpClient) : ITransactionService
    {
        public async Task<bool> AddTransactionAsync(Guid userId, Transaction transaction)
        {
            var result = await httpClient.PostAsJsonAsync($"/api/bankbook/transaction/add", transaction);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId)
        {
            var result = await httpClient.DeleteAsync($"/api/bankbook/transaction/delete/{transactionId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid userId, Guid transactionId)
        {
            var result = await httpClient.GetFromJsonAsync<Transaction?>($"/api/bankbook/transaction/get/{transactionId}");

            return result;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(Guid userId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/bankbook/transaction/list");

            return result!;
        }

        public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(Guid userId, Guid accountId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/banobook/transaction/list/account/{accountId}");

            return result!;
        }

        public async Task<List<Transaction>> GetTransactionsBySubCategoryIdAsync(Guid userId, Guid subCategoryId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Transaction>>($"/api/bankbook/transaction/list/subcategory/{subCategoryId}");

            return result!;
        }

        public async Task<bool> UpdateTransactionAsync(Guid userId, Transaction transaction)
        {
            var result = await httpClient.PutAsJsonAsync($"/api/bankbook/transaction/update", transaction);

            return result.IsSuccessStatusCode;
        }
    }
}
