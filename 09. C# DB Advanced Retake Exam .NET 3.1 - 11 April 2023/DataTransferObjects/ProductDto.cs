using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Invoices.DataTransferObjects
{
    public class ProductDto
    {

        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(5.00, 1000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 4)]
        public CategoryType CategoryType { get; set; }

        public List<int> Clients { get; set; }
    }
}
