using C3P1.Net.Client.Data.Apps.Tasklist;

namespace C3P1.Net.Client.Services.Apps
{
    public interface ITasklistService
    {
        public Task<List<TodoItem>> GetTasklistAsync(Guid userId);
        public Task<List<TodoItem>> GetTasklistTodoAsync(Guid userId);
        public Task<List<TodoItem>> GetTasklistDoneAsync(Guid userId);
        public Task<bool> AddTaskAsync(Guid userId, TodoItem task);
        public Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);
        public Task<bool> UpdateTaskAsync(TodoItem task);
        public Task<List<TodoItem>> DeleteTasklistDoneAsync(Guid userId);
        public Task<List<TodoItem>> MarkTasklistAsDoneAsync(Guid userId);
    }
}
