using C3P1.Net.Data.Models;
using C3P1.Net.Data.Services.Shared;

namespace C3P1.Net.Data.Services
{
    public interface ITasklistService
    {
        public Task<List<TodoItem>> GetTasklistAsync(Guid userId);
        public Task<List<TodoItem>> GetTodoTasklistAsync(Guid userId);
        public Task<List<TodoItem>> GetDoneTasklistAsync(Guid userId);
        public Task<TodoItem> GetTodoItemAsync(Guid userId, Guid id);
        public Task<TodoItem> AddTodoItemAsync(Guid userId, TodoItem item);
        public Task<TodoItem> UpdateTodoItemAsync(TodoItem item);
        public Task<TodoItem> DeleteTodoItemAsync(Guid userId, Guid id);
        public Task<List<TodoItem>> DeleteDoneTasklistAsync(Guid userId);
        public Task<PagedList<TodoItem>> GetTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder);
        public Task<PagedList<TodoItem>> GetTodoTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder);
        public Task<PagedList<TodoItem>> GetDoneTasklistPagedAsync(Guid userId, int? pageNumber, int pageSize, string sortField, string sortOrder);
    }
}
