// Disable warnings for nullables
#nullable disable

using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Data.Models
{
    public class TodoItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Task title must be set")]
        public string Title { get; set; }
        public bool Completed { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? DueTime { get; set; }
        public DateTime? CompletedTime { get; set; }
    }
}
