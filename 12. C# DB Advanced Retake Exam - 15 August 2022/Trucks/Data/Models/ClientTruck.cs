using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [Required]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        [Required]
        public int TruckId { get; set; }

        public Truck Truck { get; set; }
    }
}
