using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductShop.Data.Models
{
    public class Product
    {
        public Product()
        {
            CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int? BuyerId { get; set; }

        [InverseProperty("ProductsBought")]
        public User Buyer { get; set; }

        public int SellerId { get; set; }

        [InverseProperty("ProductsSold")]
        public User Seller { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
