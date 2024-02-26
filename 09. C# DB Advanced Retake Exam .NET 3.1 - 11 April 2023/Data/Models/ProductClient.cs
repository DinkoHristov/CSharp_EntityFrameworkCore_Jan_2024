using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class ProductClient
    {
        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
