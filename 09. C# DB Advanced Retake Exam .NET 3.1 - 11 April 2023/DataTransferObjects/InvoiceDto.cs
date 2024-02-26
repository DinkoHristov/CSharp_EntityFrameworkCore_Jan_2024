using Invoices.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataTransferObjects
{
    public class InvoiceDto
    {
        [Required]
        [Range(1_000_000_000, 1_500_000_000)]
        public int Number { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, 2)]
        public CurrencyType CurrencyType { get; set; }

        public int ClientId { get; set; }
    }
}
