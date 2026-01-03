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

            // add category
            _context.Add(subcategory);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid userId, Guid subcategoryId)
        {
            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.Id == subcategoryId && x.UserId == userId);
            if (subcategory != null)
            {
                _context.SubCategories.Remove(subcategory);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }

        public async Task<List<SubCategory>> GetSubCategoriesAsync(Guid userId)
        {
            var result = await _context.SubCategories
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return result;
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(Guid userId, Guid subCategoryId)
        {
            var subcategory = await _context.SubCategories
                .FirstOrDefaultAsync(x => x.Id == subCategoryId && x.UserId == userId);
            return subcategory;
        }

        public async Task<bool> UpdateSubCategoryAsync(Guid userId, SubCategory subCategory)
        {
            var existingSubCategory = await _context.SubCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == subCategory.Id && x.UserId == userId);
            if (existingSubCategory != null)
            {
                subCategory.UserId = userId; // ensure the userId is not changed
                _context.SubCategories.Update(subCategory);
                int recorded = await _context.SaveChangesAsync();
                return (recorded == 1);
            }
            return false;
        }
    }
}
