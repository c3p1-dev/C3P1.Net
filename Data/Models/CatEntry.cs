using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Data.Models
{
    public class CatEntry
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatId { get; set; }
        public DateTime CreationTime { get; set; }
        [Required(ErrorMessage = "EntryTime must be set")]
        public DateTime EntryTime { get; set; }
        public double? Weight { get; set; }
        public double? Temperature { get; set; }
        public string? Comment { get; set; }

    }
}
