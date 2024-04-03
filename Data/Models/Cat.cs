using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Data.Models
{
    public class Cat
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Name must be set")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Gender must be set")]
        public required string Gender { get; set; } // "Male" or "Female"

        [Required(ErrorMessage = "Birthdate must be set")]
        public DateTime Birthdate { get; set; }
        public string? Breed { get; set; }
        public string? Color { get; set; }
    }
}
