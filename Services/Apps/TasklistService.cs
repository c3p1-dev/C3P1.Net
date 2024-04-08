using C3P1.Net.Data;
using C3P1.Net.Data.Models;
using C3P1.Net.Services.Shared;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps
{
    public class TasklistService : ITasklistService
    {
        private readonly AppDbContext _context;
        public TasklistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> AddTodoItemAsync(Guid userId, TodoItem item)
        {
            item.Id = Guid.NewGuid();
            item.UserId = userId;
            item.CreationTime = DateTime.Now;
            _context.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<TodoItem> DeleteTodoItemAsync(Guid userId, Guid id)
        {
            TodoItem? item = await _context.Tasklist
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();

            if (item is not null)
            {
                _context.Tasklist.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            else
            {
                throw new Exception($"Item {id} does not exist");
            }
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid userId, Guid id)
        {
            TodoItem? item = await _context.Tasklist
                .Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();

            if (item is not null)
            {
                return item;
            }
            else
            {
                throw new Exception($"Item {id} does not exist");
            }
        }

        public async Task<List<TodoItem>> GetTasklistAsync(Guid userId)
        {
            return await _context.Tasklist
                .Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<TodoItem> UpdateTodoItemAsync(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<List<TodoItem>> GetTodoTasklistAsync(Guid userId)
        {

            return await _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == false).ToListAsync();
        }

        public async Task<List<TodoItem>> GetDoneTasklistAsync(Guid userId)
        {
            return await _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == true).ToListAsync();
        }

        public async Task<List<TodoItem>> DeleteDoneTasklistAsync(Guid userId)
        {
            List<TodoItem> items = await _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == true).ToListAsync();

            foreach (TodoItem item in items)
            {
                _context.Remove<TodoItem>(item);
            }

            await _context.SaveChangesAsync();

            return items;
        }

        public async Task<PagedList<TodoItem>> GetTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder)
        {
            var list = _context.Tasklist
                .Where(x => x.UserId == userId)
                .OrderByDynamic(sortField, sortOrder.ToUpper());

            return await PagedList<TodoItem>.CreateAsync(list.AsTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<PagedList<TodoItem>> GetTodoTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder)
        {
            var list = _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == false)
                .OrderByDynamic(sortField, sortOrder.ToUpper());

            return await PagedList<TodoItem>.CreateAsync(list.AsTracking(), pageNumber ?? 1, pageSize);
        }

        public async Task<PagedList<TodoItem>> GetDoneTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder)
        {
            var list = _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == true)
                .OrderByDynamic(sortField, sortOrder.ToUpper());

            return await PagedList<TodoItem>.CreateAsync(list.AsTracking(), pageNumber ?? 1, pageSize);
        }
    }
}
