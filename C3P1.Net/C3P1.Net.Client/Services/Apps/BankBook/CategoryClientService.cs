using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class CategoryClientService(HttpClient httpClient) : ICategoryService
    {
        public async Task<bool> AddCategoryAsync(Guid userId, Category category)
        {
            var result = await httpClient.PostAsJsonAsync($"/api/apps/bankbook/category/add", category);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var result = await httpClient.DeleteAsync($"/api/apps/bankbook/category/delete/{categoryId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<List<Category>> GetCategoriesAsync(Guid userId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Category>>($"/api/apps/bankbook/category/list");

            return result!;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
        {
            var result = await httpClient.GetFromJsonAsync<Category?>($"/api/apps/bankbook/category/get/{categoryId}");

            return result;
        }

        public async Task<bool> UpdateCategoryAsync(Guid userId, Category category)
        {
            var result = await httpClient.PutAsJsonAsync($"/api/apps/bankbook/category/update", category);

            return result.IsSuccessStatusCode;
        }

        public async Task<Category?> GetCategoryByCodeAsync(Guid userId, string code)
        {
            var result = await httpClient.GetFromJsonAsync<Category?>($"/api/apps/bankbook/category/get/code/{code}");
            return result;
        }
    }
}
