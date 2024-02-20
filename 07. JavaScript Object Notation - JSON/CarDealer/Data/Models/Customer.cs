using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public bool IsYoundDriver { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
