﻿using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("Sale")]
    public class SaleDto
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}
