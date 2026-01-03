using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class CategoryClientService(HttpClient httpClient) : ICategoryService
    {
        private readonly HttpClient _httpClient = httpClient;
        public async Task<bool> AddCategoryAsync(Guid userId, Category category)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/apps/bankbook/category", category);

            return result.IsSuccessStatusCode;
        }



        public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var result = await _httpClient.DeleteAsync($"/api/apps/bankbook/category/{categoryId}");

            return result.IsSuccessStatusCode;
        }



        public async Task<List<Category>> GetCategoriesAsync(Guid userId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<Category>>($"/api/apps/bankbook/category");

            return result!;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
        {
            var result = await _httpClient.GetFromJsonAsync<Category?>($"/api/apps/bankbook/category/{categoryId}");

            return result;
        }



        public async Task<bool> UpdateCategoryAsync(Guid userId, Category category)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/apps/bankbook/category", category);

            return result.IsSuccessStatusCode;
        }

    }
}
