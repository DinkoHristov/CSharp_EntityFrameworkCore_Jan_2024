using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Data.Models
{
    public class Part
    {
        public Part()
        {
            PartCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public virtual ICollection<PartCar> PartCars { get; set; }
    }
}
