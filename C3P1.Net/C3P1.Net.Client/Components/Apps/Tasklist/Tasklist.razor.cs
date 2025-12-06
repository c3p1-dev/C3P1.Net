using Blazorise;
using C3P1.Net.Shared.Data.Apps.Tasklist;
using C3P1.Net.Shared.Services.Apps;
using Microsoft.AspNetCore.Components;

namespace C3P1.Net.Client.Components.Apps.Tasklist
{
    public abstract class TasklistBase : ComponentBase
    {
        protected Filter filter = Filter.Todo;
        protected Validations? validations;
        protected string? title;
        protected Guid currentUserId;

        protected List<TodoItem>? tasklist;
        protected IEnumerable<TodoItem> Tasklist
        {
            get
            {
                if (tasklist != null)
                {
                    var query = from t in tasklist select t;

                    if (filter == Filter.Todo)
                        query = from q in query where !q.Completed select q;

                    if (filter == Filter.Done)
                        query = from q in query where q.Completed select q;

                    return query;
                }
                else // return blank list during tasklist loading
                {
                    return [];
                }
            }
        }

        [Inject]
        ITasklistService? TasklistService { get; set; }
        protected void SetFilter(Filter filter)
        {
            this.filter = filter;
        }

        protected async void AddTask()
        {
            if (await validations!.ValidateAll())
            {
                var newTask = new TodoItem()
                {
                    Completed = false,
                    Title = title ?? string.Empty,
                };

                await TasklistService!.AddTaskAsync(currentUserId, newTask);
                await LoadData();
                title = null;

                await validations.ClearAll();

                await InvokeAsync(StateHasChanged);
            }
        }

        protected async Task DeleteDoneTasks()
        {
            await TasklistService!.DeleteTasklistDoneAsync(currentUserId);
            await LoadData();

            await InvokeAsync(StateHasChanged);
        }
        protected async Task MarkAllAsDone()
        {
            await TasklistService!.MarkTasklistAsDoneAsync(currentUserId);
            await LoadData();

            await InvokeAsync(StateHasChanged);
        }
        protected async Task LoadData()
        {
            // load tasklist
            tasklist = await TasklistService!.GetTasklistAsync(currentUserId);
        }

        protected Task OnTodoStatusChanged(bool isChecked)
        {
            return InvokeAsync(StateHasChanged);
        }
    }
}
