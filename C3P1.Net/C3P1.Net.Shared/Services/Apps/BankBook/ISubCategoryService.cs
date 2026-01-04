using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface ISubCategoryService
    {
        public Task<List<SubCategory>> GetSubCategoriesAsync(Guid userId);
        public Task<bool> AddSubCategoryAsync(Guid userId, SubCategory subCategory);
        public Task<bool> DeleteSubCategoryAsync(Guid userId, Guid subCategoryId);
        public Task<bool> UpdateSubCategoryAsync(Guid userId, SubCategory subCategory);
        public Task<SubCategory?> GetSubCategoryByIdAsync(Guid userId, Guid subCategoryId);
        public Task<SubCategory?> GetSubCategoryByCodeAsync(Guid userId, string code);
        public Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(Guid userId, Guid categoryId);
        public Task<List<SubCategory>> GetSubCategoriesByCategoryCodeAsync(Guid userId, string categoryCode);
    }
}
