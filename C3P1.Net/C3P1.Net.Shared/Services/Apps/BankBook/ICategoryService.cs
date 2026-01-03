using C3P1.Net.Shared.Data.Apps.BankBook;

namespace C3P1.Net.Shared.Services.Apps.BankBook
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync(Guid userId);
        public Task<bool> AddCategoryAsync(Guid userId, Category category);
        public Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId);
        public Task<bool> UpdateCategoryAsync(Guid userId, Category category);
        public Task<Category?> GetCategoryByIdAsync(Guid userId, Guid categoryId);
    }
}
