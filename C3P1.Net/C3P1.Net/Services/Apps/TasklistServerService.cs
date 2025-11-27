
using C3P1.Net.Client.Components.Apps.Tasklist;
using C3P1.Net.Client.Data.Apps.Tasklist;
using C3P1.Net.Client.Services.Apps;
using C3P1.Net.Data;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Services.Apps
{
    public class TasklistServerService : ITasklistService
    {
        private AppDbContext _context;
        public TasklistServerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TodoItem>> GetTasklistAsync(Guid userId)
        {
            var result = await _context.Tasklist
                .Where(x => x.UserId == userId)
                .ToListAsync();

            result.Reverse();

            return result;
        }
        public async Task<List<TodoItem>> GetTasklistTodoAsync(Guid userId)
        {
            var result = await _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == false)
                .ToListAsync();

            result.Reverse();

            return result;
        }
        public async Task<List<TodoItem>> GetTasklistDoneAsync(Guid userId)
        {
            var result = await _context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == true)
                .ToListAsync();

            result.Reverse();

            return result;
        }

        public async Task<bool> AddTaskAsync(Guid userId, TodoItem task)
        {
            // fill data
            task.Id = Guid.NewGuid();
            task.UserId = userId;
            task.Completed = false;

            // add task
            _context.Add(task);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            // try to get task from taskId
            var task = await _context.Tasklist.Where(x => x.UserId == userId && x.Id == taskId).FirstOrDefaultAsync();
            if (task == null)
            {
                // task not found
                return false;
            }

            // delete task
            _context.Remove(task);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> UpdateTaskAsync(TodoItem task)
        {
            // update item
            _context.Update(task);
            int recorded = await _context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<List<TodoItem>> DeleteTasklistDoneAsync(Guid userId)
        {
            List<TodoItem> result = new List<TodoItem>();

            foreach (var task in _context.Tasklist.Where(x => x.UserId == userId && x.Completed == true))
            {
                _context.Remove(task);
                result.Add(task);
            }

            int recorded = await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<TodoItem>> MarkTasklistAsDoneAsync(Guid userId)
        {
            List<TodoItem> result = new List<TodoItem>();

            foreach (var task in _context.Tasklist.Where(x => x.UserId == userId && x.Completed == false))
            {
                task.Completed = true;
                result.Add(task);
            }

            int recorded = await _context.SaveChangesAsync();

            return result;
        }
    }
}
