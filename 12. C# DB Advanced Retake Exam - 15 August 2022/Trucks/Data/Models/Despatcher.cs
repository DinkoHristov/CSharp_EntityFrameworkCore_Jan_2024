using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.Data.Models
{
    public class Despatcher
    {
        public Despatcher()
        {
            Trucks = new HashSet<Truck>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        public string Position { get; set; }

        [XmlArray("Trucks")]
        public virtual ICollection<Truck> Trucks { get; set; }
    }
}
