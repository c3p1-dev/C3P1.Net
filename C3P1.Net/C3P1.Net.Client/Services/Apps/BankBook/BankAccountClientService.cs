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
            var result = await _httpClient.GetFromJsonAsync<List<BankAccount>>("api/apps/bankbook/bankaccount");

            return result!;
        }
        public Task<bool> AddBankAccountAsync(Guid userId, BankAccount bankAccount)
        {
            throw new NotImplementedException();
        }
    }
}
