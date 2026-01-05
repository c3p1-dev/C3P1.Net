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

            // check duplicate code
            var duplicateCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == category.Code);

            if (duplicateCategory is null)
            {
                // add category
                _context.Add(category);
                int recorded = await _context.SaveChangesAsync();

                return (recorded == 1);
            }
            else // category code duplicate case
                return false;
        }

        public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
        {
            // find category
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId && x.UserId == userId);

            // delete if found
            if (category is not null)
            {
                _context.Categories.Remove(category);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }

        public async Task<List<Category>> GetCategoriesAsync(Guid userId)
        {
            // get categories
            var result = await _context.Categories
                .Where(x => x.UserId == userId)
                .ToListAsync();

            // return result
            return result;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
        {
            // find category
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId && x.UserId == userId);

            return category;
        }

        public async Task<bool> UpdateCategoryAsync(Guid userId, Category category)
        {
            // find existing category
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == category.Id && x.UserId == userId);

            // if not found, return false
            if (existingCategory is null)
                return false;

            // check for duplicate code
            var duplicateAccount = await _context.Accounts
                .AnyAsync(x => x.UserId == userId
                            && x.Code == category.Code
                            && x.Id != category.Id);

            // if duplicate found, return false
            if (duplicateAccount == true)
                return false;

            // update fields
            existingCategory.Code = category.Code;
            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            // save changes
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category?> GetCategoryByCodeAsync(Guid userId, string code)
        {
            // find category by code
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);

            return category;
        }
    }
}
