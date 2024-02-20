﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarDealer.Data.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public int Discount { get; set; }

        [Required]
        public int CarId { get; set; }

        public Car Car { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
