using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Shared.Data.Apps.BankBook
{
    public enum PaymentMethod
    {
        Card,
        Transfer,
        DirectDebit,
        Cash,
        Other
    }
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "User ID must be defined")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Account ID must be defined")]
        public Guid AccountId { get; set; }

        [Required(ErrorMessage = "SubCategory ID must be defined")]
        public Guid SubCategoryId { get; set; }

        [Required(ErrorMessage = "Label is required")]
        [StringLength(150, ErrorMessage = "Label is at most 150 characters")]
        public string Label { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Note is at most 255 characters")]
        public string Note { get; set; } = string.Empty;

        [Required(ErrorMessage = "CreatedAt must be defined")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "AccountingDate must be defined")]
        public DateOnly AccountingDate { get; set; }
        public DateOnly? ValueDate { get; set; }

        [Required(ErrorMessage = "PaymentMethod must be defined")]
        public PaymentMethod PaymentMethod { get; set; }

        public bool IsReconciled { get; set; } = false;

        [Required(ErrorMessage = "Amount must be defined")]
        public decimal Amount { get; set; }
    }
}
