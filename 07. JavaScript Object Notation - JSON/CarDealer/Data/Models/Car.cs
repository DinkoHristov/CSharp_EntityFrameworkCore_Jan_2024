using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Data.Models
{
    public class Car
    {
        public Car()
        {
            PartCars = new HashSet<PartCar>();
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int TravelledDistance { get; set; }

        public virtual ICollection<PartCar> PartCars { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
