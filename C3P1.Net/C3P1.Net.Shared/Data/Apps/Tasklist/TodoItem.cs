using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Shared.Data.Apps.Tasklist
{
    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Task title must be set")]
        public string? Title { get; set; }
        public bool Completed { get; set; }

    }
}
