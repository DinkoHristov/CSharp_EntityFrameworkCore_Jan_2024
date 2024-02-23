using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("partId")]
    public class PartCarDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
