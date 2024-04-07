using C3P1.Net.Data;
using C3P1.Net.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps
{
    public class CatService : ICatService
    {
        private readonly ApplicationDbContext _context;
        public CatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cat> AddCatAsync(Guid userId, Cat cat)
        {
            cat.Id = Guid.NewGuid();
            cat.UserId = userId;

            _context.Add(cat);
            await _context.SaveChangesAsync();

            return cat;
        }

        public async Task<List<Cat>> GetCatsAsync(Guid userId)
        {
            return await _context.Cats
                .Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Cat> DeleteCatAsync(Guid userId, Guid id)
        {
            Cat? cat = await _context.Cats
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();

            if (cat is not null)
            {
                _context.Cats.Remove(cat);
                await _context.SaveChangesAsync();
                return cat;
            }
            else
            {
                throw new Exception($"Cat {id} does not exist");
            }
        }
        public async Task<Cat> UpdateCatAsync(Cat cat)
        {
            _context.Entry(cat).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return cat;
        }

        public async Task<Cat> GetCatAsync(Guid userId, Guid id)
        {
            Cat? cat = await _context.Cats
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();

            if (cat is not null)
            {
                return cat;
            }
            else
            {
                throw new Exception($"Cat {id} does not exist");
            }
        }

        public async Task<List<CatEntry>> GetCatEntriesAsync(Guid userId, Guid catId)
        {
            return await _context.CatEntries
                .Where(x => x.UserId == userId && x.CatId == catId).ToListAsync();
        }

        public async Task<CatEntry> AddCatEntryAsync(Guid userId, Guid catId, CatEntry entry)
        {
            entry.Id = Guid.NewGuid();
            entry.CatId = catId;
            entry.UserId = userId;

            _context.Add(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        public async Task<CatEntry> DeleteCatEntryAsync(Guid userId, Guid catId, Guid id)
        {
            CatEntry? entry = await _context.CatEntries
                .Where(x => x.UserId == userId && x.CatId == catId && x.Id == id).FirstOrDefaultAsync();

            if (entry is not null)
            {
                _context.CatEntries.Remove(entry);
                await _context.SaveChangesAsync();
                return entry;
            }
            else
            {
                throw new Exception($"Entry {id} does not exist");
            }
        }

        public async Task<CatEntry> UpdateCatEntryAsync(CatEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entry;
        }

        public async Task<CatEntry> GetCatEntryAsync(Guid userId, Guid catId, Guid id)
        {
            CatEntry? entry = await _context.CatEntries
                .Where(x => x.UserId == userId && x.Id == id && x.CatId == catId).FirstOrDefaultAsync();

            if (entry is not null)
            {
                return entry;
            }
            else
            {
                throw new Exception($"CatEntry {id} does not exist");
            }
        }

        public async Task<List<CatEntry>> GetCatWeightListAsync(Guid userId, Guid catId)
        {
            return await _context.CatEntries
                .Where(x => x.UserId == userId && x.CatId == catId && (x.Weight != null)).ToListAsync();
        }
    }
}
