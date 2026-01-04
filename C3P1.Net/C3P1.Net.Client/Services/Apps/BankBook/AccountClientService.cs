using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class AccountClientService(HttpClient httpClient) : IAccountService
    {
        private readonly HttpClient _httpClient = httpClient;
        public async Task<List<Account>> GetAccountsAsync(Guid userId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<Account>>("api/apps/bankbook/account/list");

            return result!;
        }
        public async Task<bool> AddAccountAsync(Guid userId, Account bankAccount)
        {
            var result = await _httpClient.PostAsJsonAsync<Account>($"api/apps/bankbook/account/add", bankAccount);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAccountAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _httpClient.DeleteAsync($"api/apps/bankbook/account/delete/{bankAccountId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAccountAsync(Guid userId, Account bankAccount)
        {
            var result = await _httpClient.PutAsJsonAsync<Account>("api/apps/bankbook/account/update", bankAccount);

            return result.IsSuccessStatusCode;
        }

        public async Task<Account?> GetAccountByIdAsync(Guid userId, Guid bankAccountId)
        {
            var result = await _httpClient.GetFromJsonAsync<Account>($"api/apps/bankbook/account/get/{bankAccountId}");
            return result;
        }

        public async Task<Account?> GetAccountByCodeAsync(Guid userId, string code)
        {
            var result = await _httpClient.GetFromJsonAsync<Account>($"api/apps/bankbook/account/get/code/{code}");
            return result;
        }
    }
}
