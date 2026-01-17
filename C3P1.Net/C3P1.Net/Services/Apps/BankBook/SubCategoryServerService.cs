using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.BankBook;
using C3P1.Net.Shared.Services.Apps.BankBook;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps.BankBook
{
    public class SubCategoryServerService(BankBookDbContext context) : ISubCategoryService
    {
        private readonly BankBookDbContext _context = context;
        public async Task<bool> AddSubCategoryAsync(Guid userId, SubCategory subcategory)
        {
            // fill data
            subcategory.Id = Guid.NewGuid(); ;
            subcategory.UserId = userId;

            // duplicate check
            var duplicateSubcategory = await _context.SubCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == subcategory.Code);

            if (duplicateSubcategory is null)
            {
                // add category
                _context.Add(subcategory);
                int recorded = await _context.SaveChangesAsync();

                return (recorded == 1);
            }

            // duplication case
            return false;
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid userId, Guid subcategoryId)
        {
            // find subcategory
            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.Id == subcategoryId && x.UserId == userId);

            // verify transaction ownership
            var ownedTransactions = await _context.Transactions
                .AsNoTracking()
                .AnyAsync(t => t.SubCategoryId == subcategoryId);

            // abort deletion if the subcategory holds transactions
            if (ownedTransactions == true)
                return false;

            // delete if found
            if (subcategory is not null)
            {
                _context.SubCategories.Remove(subcategory);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }

            return false;
        }

        public async Task<List<SubCategory>> GetSubCategoriesAsync(Guid userId)
        {
            // get subcategories
            var result = await _context.SubCategories
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(Guid userId, Guid subCategoryId)
        {
            // get subcategory
            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.Id == subCategoryId && x.UserId == userId);

            return subcategory;
        }

        public async Task<bool> UpdateSubCategoryAsync(Guid userId, SubCategory subCategory)
        {
            // find existing sub category
            var existingSubCategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.Id == subCategory.Id && x.UserId == userId);

            // if not found, return false
            if (existingSubCategory is null)
                return false;

            // check for duplicate code
            var duplicateAccount = await _context.SubCategories
                .AnyAsync(x => x.UserId == userId
                            && x.Code == subCategory.Code
                            && x.Id != subCategory.Id);

            // if duplicate found, return false
            if (duplicateAccount == true)
                return false;

            // update fields
            existingSubCategory.Code = subCategory.Code;
            existingSubCategory.Name = subCategory.Name;
            existingSubCategory.Description = subCategory.Description;
            existingSubCategory.CategoryId = subCategory.CategoryId;

            // save changes
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SubCategory?> GetSubCategoryByCodeAsync(Guid userId, string code)
        {
            // get subcategory by code
            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);

            return subcategory;
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryIdAsync(Guid userId, Guid categoryId)
        {
            // get subcategories by category id
            var result = await _context.SubCategories
                .Where(x => x.UserId == userId && x.CategoryId == categoryId)
                .ToListAsync();

            return result;
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryCodeAsync(Guid userId, string categoryCode)
        {
            // get category by code
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Code == categoryCode);

            if (category is not null)
            {
                // get subcategories by category id
                var result = await _context.SubCategories
                    .Where(x => x.UserId == userId && x.CategoryId == category.Id)
                    .ToListAsync();

                return result;
            }

            return [];
        }
    }
}
