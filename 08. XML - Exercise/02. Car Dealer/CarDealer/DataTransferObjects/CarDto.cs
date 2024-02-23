using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("Car")]
    public class CarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public int TravelledDistance { get; set; }

        [XmlArray("parts")]
        public List<PartCarDto> PartCars { get; set; }
    }
}
