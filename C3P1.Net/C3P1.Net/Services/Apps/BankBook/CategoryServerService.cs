using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps.BankBook
{
    public class CategoryServerService(BankBookDbContext context) : ICategoryService
    {
        private readonly BankBookDbContext _context = context;
        public async Task<bool> AddCategoryAsync(Guid userId, Category category)
        {
            // fill data
            category.Id = Guid.NewGuid(); ;
            category.UserId = userId;

            // add category
            _context.Add(category);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId && x.UserId == userId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }

        public async Task<List<Category>> GetCategoriesAsync(Guid userId)
        {
            var result = await _context.Categories
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId && x.UserId == userId);
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Guid userId, Category category)
        {
            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == category.Id && x.UserId == userId);
            if (existingCategory != null)
            {
                category.UserId = userId; // ensure the userId is not changed
                _context.Categories.Update(category);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }
    }
}
