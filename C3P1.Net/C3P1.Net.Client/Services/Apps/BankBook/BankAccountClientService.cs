using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class BankAccountClientService(HttpClient httpClient) : IBankAccountService
    {
        private readonly HttpClient _httpClient = httpClient;
        public async Task<List<BankAccount>> GetBankAccountsAsync(Guid userId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<BankAccount>>("api/apps/bankbook/bank");

            return result!;
        }
        public async Task<bool> AddBankAccountAsync(Guid userId, BankAccount bankAccount)
        {
            var result = await _httpClient.PostAsJsonAsync<BankAccount>($"api/apps/bankbook/bank", bankAccount);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBankAccountAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _httpClient.DeleteAsync($"api/apps/bankbook/bank/{bankAccountId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateBankAccountAsync(Guid userId, BankAccount bankAccount)
        {
            var result = await _httpClient.PutAsJsonAsync<BankAccount>("api/apps/bankbook/bank", bankAccount);

            return result.IsSuccessStatusCode;
        }

        public async Task<BankAccount?> GetBankAccountByIdAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _httpClient.GetFromJsonAsync<BankAccount>($"api/apps/bankbook/bank/{bankAccountId}");
            return result;
        }
    }
}
