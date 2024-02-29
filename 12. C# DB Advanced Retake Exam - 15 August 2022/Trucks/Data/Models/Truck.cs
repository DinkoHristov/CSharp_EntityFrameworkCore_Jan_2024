using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        public int Id { get; set; }

        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression("[A-Z]{2}[0-9]{4}[A-Z]{2}")]
        public string RegistrationNumber { get; set; }

        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinNumber { get; set; }

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        public int DespatcherId { get; set; }

        public Despatcher Despatcher { get; set; }

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
