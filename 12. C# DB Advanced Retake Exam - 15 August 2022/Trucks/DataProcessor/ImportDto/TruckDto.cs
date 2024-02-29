using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckDto
    {
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression("[A-Z]{2}[0-9]{4}[A-Z]{2}")]
        public string RegistrationNumber { get; set; }

        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinNumber { get; set; }

        [Required]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Required]
        [Range(5000, 29000)]
        public int CargoCapacity { get; set; }

        [Required]
        [Range(0, 3)]
        public int CategoryType { get; set; }

        [Required]
        [Range(0, 4)]
        public int MakeType { get; set; }
    }
}
