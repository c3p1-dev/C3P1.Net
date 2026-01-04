using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using System.Net.Http.Json;

namespace C3P1.Net.Client.Services.Apps.BankBook
{
    public class SubCategoryClientService(HttpClient httpClient) : ISubCategoryService
    {
        private readonly HttpClient _httpClient = httpClient;
        public async Task<bool> AddSubCategoryAsync(Guid userId, SubCategory subCategory)
        {
            var result = await _httpClient.PostAsJsonAsync($"/api/apps/bankbook/subcategory/add", subCategory);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid userId, Guid subCategoryId)
        {
            var result = await _httpClient.DeleteAsync($"/api/apps/bankbook/subcategory/delete/{subCategoryId}");

            return result.IsSuccessStatusCode;
        }

        public async Task<List<SubCategory>> GetSubCategoriesAsync(Guid userId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<SubCategory>>($"/api/apps/bankbook/subcategory/list");

            return result!;
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(Guid userId, Guid subCategoryId)
        {
            var result = await _httpClient.GetFromJsonAsync<SubCategory?>($"/api/apps/bankbook/subcategory/get/{subCategoryId}");

            return result;
        }

        public async Task<bool> UpdateSubCategoryAsync(Guid userId, SubCategory subCategory)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/apps/bankbook/subcategory/update", subCategory);

            return result.IsSuccessStatusCode;
        }

        public async Task<SubCategory?> GetSubCategoryByCodeAsync(Guid userId, string code)
        {
            var result = await _httpClient.GetFromJsonAsync<SubCategory?>($"/api/apps/bankbook/subcategory/get/code/{code}");

            return result;
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(Guid userId, Guid categoryId)
        {
            var result = await _httpClient.GetFromJsonAsync<List<SubCategory>>($"/api/apps/bankbook/subcategory/list/category/{categoryId}");

            return result!;
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryCodeAsync(Guid userId, string categoryCode)
        {
            var result = await _httpClient.GetFromJsonAsync<List<SubCategory>>($"/api/apps/bankbook/subcategory/list/category/code/{categoryCode}");

            return result!;
        }
    }
}
