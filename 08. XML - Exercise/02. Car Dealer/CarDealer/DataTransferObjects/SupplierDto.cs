﻿using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("Supplier")]
    public class SupplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}
