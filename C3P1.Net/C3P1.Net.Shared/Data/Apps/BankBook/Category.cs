using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Shared.Data.Apps.BankBook
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User ID must be defined")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Category Code is required")]
        [StringLength(10, ErrorMessage = "Category Code is at most 10 characters")]
        [RegularExpression("^[A-Z0-9]{1,10}$", ErrorMessage = "Invalid format : A-Z and 0-9, at most 10 characters")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Category Name is at most 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Description is at most 100 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
