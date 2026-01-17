using C3P1.Net.Data;
using C3P1.Net.Shared.Data.Apps.Tasklist;
using C3P1.Net.Shared.Services.Apps;
using Microsoft.EntityFrameworkCore;

namespace C3P1.Net.Services.Apps
{
    public class TasklistServerService(AppDbContext context) : ITasklistService
    {
        public async Task<List<TodoItem>> GetTasklistAsync(Guid userId)
        {
            // get all tasks
            var result = await context.Tasklist
                .Where(x => x.UserId == userId)
                .ToListAsync();

            result.Reverse();

            return result;
        }
        public async Task<List<TodoItem>> GetTasklistTodoAsync(Guid userId)
        {
            // get all todo tasks
            var result = await context.Tasklist
                .Where(x => x.UserId == userId && x.Completed == false)
                .ToListAsync();

            result.Reverse();

            return result;
        }
        public async Task<List<TodoItem>> GetTasklistDoneAsync(Guid userId)
        {
            // get all done tasks
            var result = await context.Tasklist
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
            context.Add(task);
            int recorded = await context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            // try to get task from taskId
            var task = await context.Tasklist.Where(x => x.UserId == userId && x.Id == taskId).FirstOrDefaultAsync();

            if (task is null)
                return false;  // task not found

            // delete task
            context.Remove(task);
            int recorded = await context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<bool> UpdateTaskAsync(TodoItem task)
        {
            // update item
            context.Update(task);
            int recorded = await context.SaveChangesAsync();

            return (recorded == 1);
        }

        public async Task<List<TodoItem>> DeleteTasklistDoneAsync(Guid userId)
        {
            List<TodoItem> result = [];

            // delete all done tasks
            foreach (var task in context.Tasklist.Where(x => x.UserId == userId && x.Completed == true))
            {
                context.Remove(task);
                result.Add(task);
            }

            int recorded = await context.SaveChangesAsync();

            return result;
        }

        public async Task<List<TodoItem>> MarkTasklistAsDoneAsync(Guid userId)
        {
            List<TodoItem> result = [];

            // mark all tasks as done
            foreach (var task in context.Tasklist.Where(x => x.UserId == userId && x.Completed == false))
            {
                task.Completed = true;
                result.Add(task);
            }

            int recorded = await context.SaveChangesAsync();

            return result;
        }
    }
}
