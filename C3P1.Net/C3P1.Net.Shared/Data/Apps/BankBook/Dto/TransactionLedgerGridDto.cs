using System;
using System.ComponentModel.DataAnnotations;

namespace C3P1.Net.Shared.Data.Apps.BankBook.Dto
{
    public class TransactionGridDto
    {
        [Required]
        public DateOnly AccountingDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        [StringLength(150)]
        public string Label { get; set; } = string.Empty;

        [Required]
        [StringLength(10, ErrorMessage = "SubCategory Code is at most 10 characters")]
        [RegularExpression("^[A-Z0-9_\\-!?@€$%]{1,10}$", ErrorMessage = "Invalid format : A-Z and 0-9, !?@€$% only, at most 10 characters")]
        public string SubCategoryCode { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string? CheckNumber { get; set; }
        public string? Note { get; set; }
        public DateOnly? ValueDate { get; set; } = DateOnly.FromDateTime(DateTime.Today); // +/- .AddDay(x)

        [Required]
        [CustomValidation(typeof(TransactionGridDto), nameof(ValidateAmount))]
        public decimal Amount { get; set; }

        public bool IsReconciled { get; set; } = false;

        public static ValidationResult? ValidateAmount(decimal amount, ValidationContext context)
        {
            if (amount == 0)
                return new ValidationResult("Amount cannot be zero");

            return ValidationResult.Success;
        }

    }
}
