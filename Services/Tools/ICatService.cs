using C3P1.Net.Data.Models;

namespace C3P1.Net.Services.Tools
{
    public interface ICatService
    {
        public Task<Cat> AddCatAsync(Guid userId, Cat cat);
        public Task<List<Cat>> GetCatsAsync(Guid userId);
        public Task<Cat> DeleteCatAsync(Guid userId, Guid id);
        public Task<Cat> UpdateCatAsync(Cat cat);
        public Task<Cat> GetCatAsync(Guid userId, Guid id);
        public Task<CatEntry> AddCatEntryAsync(Guid userId, Guid catId, CatEntry entry);
        public Task<List<CatEntry>> GetCatEntriesAsync(Guid userId, Guid catId);
        public Task<CatEntry> DeleteCatEntryAsync(Guid userId, Guid catId, Guid id);
        public Task<CatEntry> UpdateCatEntryAsync(CatEntry entry);
        public Task<CatEntry> GetCatEntryAsync(Guid userId, Guid catId, Guid id);
        public Task<List<CatEntry>> GetCatWeightListAsync(Guid userId, Guid catId);
    }
}
