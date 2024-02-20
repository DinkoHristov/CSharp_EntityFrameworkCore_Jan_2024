using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Data.Models
{
    public class Supplier
    {
        public Supplier()
        {
            Parts = new HashSet<Part>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsImported { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
    }
}
